// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

namespace NXTBackend.API.Core.Services;

public sealed class GitService(
    DatabaseContext ctx,
    IConfiguration config,
    ILogger<GitService> logger,
    IHttpClientFactory clientFactory,
    IHttpContextAccessor httpContext) : BaseService<Git>(ctx), IGitService
{
    private readonly HttpClient _client = clientFactory.CreateClient("NXTGit");

    private readonly string _gitTemplate = config.GetValue<string>("GitTemplate")
            ?? throw new InvalidDataException("A git template is required!");

    private Guid GetUserID()
    {
        var ctx = httpContext.HttpContext;
        string? claim = ctx?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var id = Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;
        if (id == Guid.Empty)
            throw new ServiceException(StatusCodes.Status401Unauthorized, "Unauthorized");
        return id;
    }

    private void SetWebauthHeader(string User)
    {
        if (_client.DefaultRequestHeaders.Contains("X-WEBAUTH-USER"))
            _client.DefaultRequestHeaders.Remove("X-WEBAUTH-USER");
        _client.DefaultRequestHeaders.Add("X-WEBAUTH-USER", User);
    }

    public async Task<string> GetFile(Guid id, string Path, string Branch = "main")
    {
        var git = await FindByIdAsync(id) ?? throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
        var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/contents/{Path}?ref={Branch}");
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadFromJsonAsync<RepoFileContentsDO>()
                ?? throw new ServiceException();
            return body.Content;
        }

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => throw new ServiceException(StatusCodes.Status404NotFound, "Not found"),
            _ => throw new ServiceException(StatusCodes.Status500InternalServerError, "Something went wrong...")
        };
    }

    public async Task<bool> AddCollaborator(Guid entityId, Guid userId)
    {
        var result = await GetUserAndGit(entityId, userId);
        var git = result.Item1;
        var user = result.Item2;
        if (git is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "Remote does not exist");
        if (user is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "User does not exist");

        SetWebauthHeader(user.Login);
        var response = await _client.PutAsJsonAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}", new
        {
            // TODO: Propergate the permissions somehow, for now default to just write (no repo is private anyway for subjects)
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

    public async Task<(Git?, User?)> IsCollaborator(Guid entityId, Guid userId)
    {
        var result = await GetUserAndGit(entityId, userId);
        var git = result.Item1;
        var user = result.Item2;
        if (git is null || user is null)
            return result;

        // NOTE(W2): Using the GIT service as a source of truth to sync collaborators.
        SetWebauthHeader(user.Login);
        var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}");
        return response.StatusCode switch
        {
            HttpStatusCode.NoContent => (git, user), // User is a collaborator
            HttpStatusCode.Unauthorized => (git, null), // TODO: Is this correct ? API Does not document.
            HttpStatusCode.NotFound => (git, null), // User is not a collaborator
            _ => throw new ServiceException()
        };
    }

    public async Task<bool> RemoveCollaborator(Guid entityId, Guid userId)
    {
        var result = await GetUserAndGit(entityId, userId);
        var git = result.Item1;
        var user = result.Item2;
        if (git is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "Remote does not exist");
        if (user is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "User does not exist");

        SetWebauthHeader(user.Login);
        var response = await _client.DeleteAsync($"/api/v1/repos/{git.Namespace}/collaborators/{user.Login}");
        return response.StatusCode switch
        {
            HttpStatusCode.NoContent => true, // User is a collaborator
            HttpStatusCode.Unauthorized => false, // TODO: Is this correct ? API Does not document.
            HttpStatusCode.NotFound => false, // User is not a collaborator
            _ => throw new ServiceException()
        };
    }

    public async Task SetFile(Guid id, string Path, string Content, string CommitMessage, string Branch = "main")
    {
        var result = await GetUserAndGit(id, GetUserID());
        var git = result.Item1 ?? throw new ServiceException(StatusCodes.Status404NotFound, "Git not found");
        var user = result.Item2 ?? throw new ServiceException(StatusCodes.Status404NotFound, "User not found");

        SetWebauthHeader(user.Login);
        var response = await _client.GetAsync($"/api/v1/repos/{git.Namespace}/contents/{Path}?ref={Branch}");
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadFromJsonAsync<RepoFileContentsDO>() ?? throw new ServiceException();
            var setFileResponse = await _client.PutAsJsonAsync($"/api/v1/repos/{git.Namespace}/contents/{Path}", new
            {
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(Content)),
                Branch,
                body.Sha,
                Message = CommitMessage,
                Author = new
                {
                    Name = user.Login
                },
            });

            var responseContent = await setFileResponse.Content.ReadAsStringAsync();
            logger.LogInformation("Response status: {Status}, content: {Content}", setFileResponse.StatusCode, responseContent);
            if (setFileResponse.IsSuccessStatusCode)
                return;
            throw new ServiceException();
        }

        throw response.StatusCode switch
        {
            HttpStatusCode.NotFound => new ServiceException(StatusCodes.Status404NotFound, "Not found"),
            _ => new ServiceException(StatusCodes.Status500InternalServerError, "Something went wrong...")
        };
    }

    public async Task<Git> UpdateRepository(Guid id, GitRepoPatchRequestDTO DTO)
    {
        GetUserID();
        var git = await FindByIdAsync(id) ?? throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
        var response = await _client.PatchAsJsonAsync($"/api/v1/repos/{git.Namespace}", DTO);

        if (response.IsSuccessStatusCode)
            return git;
        throw response.StatusCode switch
        {
            HttpStatusCode.Forbidden => new ServiceException(StatusCodes.Status403Forbidden, "Forbidden"),
            HttpStatusCode.NotFound => new ServiceException(StatusCodes.Status404NotFound, "Not Found"),
            _ => new ServiceException(),
        };
    }

    public async Task<Git> CreateRepository(GitRepoPostRequestDTO DTO, OwnerKind OwnerType)
    {
        GetUserID();
        var response = await _client.PostAsJsonAsync($"/api/v1/repos/{_gitTemplate}/generate", DTO);
        // logger.LogInformation("Response: {response}", response);

        if (response.StatusCode is HttpStatusCode.Conflict)
            throw new ServiceException(StatusCodes.Status409Conflict, "The repository with the same name already exists");

        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<RepoDO>() ??
            throw new ServiceException(StatusCodes.Status500InternalServerError, "Model mistmatch");
        var git = new Git
        {
            Name = model.Name,
            Namespace = model.FullName,
            Url = model.CloneUrl,
            Branch = model.DefaultBranch,
            ProviderType = GitProviderKind.Managed, // This as well needs to be configurable
            OwnerType = OwnerType
        };

        return await CreateAsync(git);
    }

    public async Task DeleteRepository(Guid id)
    {
        GetUserID();
        var git = await FindByIdAsync(id) ?? throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
        var response = await _client.DeleteAsync($"/api/v1/repos/{git.Namespace}");
        if (response.IsSuccessStatusCode)
            return;

        throw response.StatusCode switch
        {
            HttpStatusCode.Forbidden => new ServiceException(StatusCodes.Status403Forbidden, "Forbidden"),
            HttpStatusCode.NotFound => new ServiceException(StatusCodes.Status404NotFound, "Not Found"),
            _ => new ServiceException(),
        };
    }

    private async Task<(Git?, User?)> GetUserAndGit(Guid entityId, Guid userId)
    {
        var query = from g in _dbSet.AsNoTracking()
                    where g.Id == entityId
                    from u in _context.Users.AsNoTracking()
                    where u.Id == userId // Cross Join
                    select new { Git = g, User = u };

        var result = await query.FirstOrDefaultAsync();
        return result is null ? (null, null) : (result.Git, result.User);
    }
}
