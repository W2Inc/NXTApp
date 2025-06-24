// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NXTBackend.API.Core.Options;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.ExternalGit;
using NXTBackend.API.Models.Responses.Gitea;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Services;

public sealed class GitService2 : BaseService<Git>, IGitService
{
	private readonly ILogger<GitService2> _logger;
	private readonly IHttpClientFactory _clientFactory;
	private readonly IHttpContextAccessor _httpContext;
	private readonly HttpClient _client;
	private readonly GitRemoteOptions _gitOptions;

	public GitService2(
		DatabaseContext ctx,
		IOptions<GitRemoteOptions> gitOptions,
		ILogger<GitService2> logger,
		IHttpClientFactory clientFactory,
		IHttpContextAccessor httpContext) : base(ctx)
	{
		_logger = logger;
		_clientFactory = clientFactory;
		_httpContext = httpContext;
		_gitOptions = gitOptions.Value;
		_client = clientFactory.CreateClient();
	}

	/// <inheritdoc />
	public async Task<Git> CreateRepository(string name, string owner, string? description = null,
		string defaultBranch = "main", OwnerKind type = OwnerKind.User)
	{
		var user = await _context.Users.AsNoTracking()
			.FirstOrDefaultAsync(u => u.Id == GetUserId());
		if (user is null)
			throw new ServiceException(StatusCodes.Status404NotFound, "User not found");


		SetAuthHeaders(user.Login);
        var response = await _client.PostAsJsonAsync($"/api/v1/repos/{_gitOptions.Template}/generate", new GitRepoPostRequestDTO()
		{
			Name = name,
			Description = description,
			Avatar = true,
			DefaultBranch = defaultBranch,
			Owner = owner
		});

        _logger.LogInformation("Response: {response}", response);

        if (response.StatusCode is HttpStatusCode.Conflict)
            throw new ServiceException(StatusCodes.Status409Conflict, "Repository already exists");

        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<RepoDO>() ??
            throw new ServiceException(StatusCodes.Status500InternalServerError, "Model mismatch");

        return await CreateAsync(new Git
        {
            Name = model.Name,
            Namespace = model.FullName,
            Url = model.CloneUrl,
            Branch = model.DefaultBranch,
            ProviderType = GitProviderKind.Managed, // This as well needs to be configurable
            OwnerType = type
        });
	}

	/// <inheritdoc />
	public async Task<Git> ArchiveRepository(Guid id)
	{
		var (git, user) = Check(await GetContext(id));

		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<GitVfsNodeDO>> GetFiles(Guid id, string path, string branch)
	{
        var git = await FindByIdAsync(id) ?? throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
        var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/contents/{path}?ref={branch}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IEnumerable<GitVfsNodeDO>>() ?? throw new ServiceException();
        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => throw new ServiceException(StatusCodes.Status404NotFound, "Not found"),
            _ => throw new ServiceException(StatusCodes.Status500InternalServerError, "Something went wrong...")
        };
	}

    /// <inheritdoc />
	public async Task<IEnumerable<GitRefDO>> GetRefs(Guid id)
	{
        var git = await FindByIdAsync(id) ?? throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
        var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/git/refs");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IEnumerable<GitRefDO>>() ?? throw new ServiceException();
        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => throw new ServiceException(StatusCodes.Status404NotFound, "Not found"),
            _ => throw new ServiceException(StatusCodes.Status500InternalServerError, "Something went wrong...")
        };
	}

	/// <inheritdoc />
	public async Task<Git> UpsertFile(Guid id, string path, string? content, string message, string branch)
	{
		var (git, user) = Check(await GetContext(id));

		var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/contents/{path}?ref={branch}");
        if (!response.IsSuccessStatusCode)
		{
            throw response.StatusCode switch
            {
                HttpStatusCode.NotFound => new ServiceException(StatusCodes.Status404NotFound, "Not found"),
                _ => new ServiceException(StatusCodes.Status500InternalServerError, "Something went wrong...")
            };
		}

        var body = await response.Content.ReadFromJsonAsync<RepoFileContentsDO>() ?? throw new ServiceException();
			
        HttpResponseMessage actionResponse;
        if (content is null)
            actionResponse = await _client.DeleteAsync($"/api/v1/repos/{git.Namespace}/contents/{path}");
        else
        {
            // Update the file with new content
            actionResponse = await _client.PutAsJsonAsync($"/api/v1/repos/{git.Namespace}/contents/{path}", new
            {
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
                branch,
                body.Sha,
                Message = message,
                Author = new
                {
                    Name = user.Login
                }
            });
        }

        var responseContent = await actionResponse.Content.ReadAsStringAsync();
        _logger.LogInformation("Response status: {Status}, content: {Content}", actionResponse.StatusCode, responseContent);
        return actionResponse.IsSuccessStatusCode ? git : throw new ServiceException();

    }

	/// <inheritdoc />
	public async Task<(Git?, User?)> IsCollaborator(Guid entityId, Guid userId)
	{
		var result = await GetContext(entityId, userId);
		var git = result.Item1;
		var user = result.Item2;
		if (git is null || user is null)
			return result;

		// NOTE(W2): Using the GIT service as a source of truth to sync collaborators.
		SetAuthHeaders(user.Login);
		var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}");
		return response.StatusCode switch
		{
			HttpStatusCode.NoContent => (git, user), // User is a collaborator
			HttpStatusCode.Unauthorized => (git, null), // User is not authenticated
			HttpStatusCode.NotFound => (git, null), // User is not a collaborator
			_ => throw new ServiceException()
		};
	}

	/// <inheritdoc />
	public async Task<bool> AddCollaborator(Guid entityId, Guid userId)
	{
		var (git, user) = Check(await GetContext(entityId, userId));
		
		var response = await _client.PutAsJsonAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}", new
		{
			// NOTE(W2): Not sure if we want more fine grain later.
			Permission = "write"
		});

		return response.StatusCode switch
		{
			HttpStatusCode.NoContent => true,
			HttpStatusCode.Unauthorized => false,
			HttpStatusCode.NotFound => false,
			HttpStatusCode.Forbidden => throw new ServiceException(StatusCodes.Status403Forbidden, "Forbidden"),
			_ => throw new ServiceException()
		};
	}

	/// <inheritdoc />
	public async Task<bool> RemoveCollaborator(Guid entityId, Guid userId)
	{
		var (git, user) = Check(await GetContext(entityId, userId));
		
		var response = await _client.DeleteAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}");
		return response.StatusCode switch
		{
			HttpStatusCode.NoContent => true, // Successfully removed
			HttpStatusCode.Unauthorized => false, // Not authenticated
			HttpStatusCode.NotFound => false, // User is not a collaborator
			_ => throw new ServiceException()
		};
	}

	/// <summary>
	/// /// Gets the current user's ID from HTTP context claims.
	/// </summary>
	/// <returns>The current user's ID or throws an exception if unauthorized</returns>
	private Guid GetUserId()
	{
		var claim = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
		var id = Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;
		return id == Guid.Empty ? throw new ServiceException(StatusCodes.Status401Unauthorized, "Unauthorized") : id;
    }

	/// <summary>
	/// Get the context data for the given entity and user.
	/// Basically required for *almost* all operations to ensure the user has access to the repository.
	/// 
	/// Each action requires the user webauth header to be set.
	/// </summary>
	/// <param name="entityId"></param>
	/// <param name="userId"></param>
	/// <returns>The user that is performing the action + the git repository info</returns>
	private async Task<(Git?, User?)> GetContext(Guid entityId, Guid userId = default)
	{
		// Use provided userId or get current user id
		var id = userId != Guid.Empty ? userId : GetUserId();

		var query = from g in _dbSet.AsNoTracking()
			where g.Id == entityId
			from u in _context.Users.AsNoTracking()
			where u.Id == id // Cross Join
			select new { Git = g, User = u };

		var result = await query.FirstOrDefaultAsync();
		return (result?.Git, result?.User);
	}

	/// <summary>
	/// Ensures the context has valid Git and User objects and sets up authentication headers.
	/// </summary>
	/// <param name="context">The context tuple to validate</param>
	/// <returns>The same context tuple for chaining</returns>
	private (Git, User) Check((Git?, User?) context)
	{
		if (context.Item1 is null)
			throw new ServiceException(StatusCodes.Status404NotFound, "Repository not found");
		if (context.Item2 is null)
			throw new ServiceException(StatusCodes.Status404NotFound, "User not found");

		SetAuthHeaders(context.Item2.Login);
		return (context.Item1, context.Item2);
	}

	/// <summary>
	/// Sets authentication headers for the HTTP client.
	/// </summary>
	/// <param name="login">The user login to use for authentication</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetAuthHeaders(string login)
	{
		if (_client.DefaultRequestHeaders.Contains("X-WEBAUTH-USER"))
			_client.DefaultRequestHeaders.Remove("X-WEBAUTH-USER");
		_client.DefaultRequestHeaders.Add("X-WEBAUTH-USER", login);
	}
}
