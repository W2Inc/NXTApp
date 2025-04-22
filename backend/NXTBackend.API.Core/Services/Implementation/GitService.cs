// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
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

    private void SetAuthorizationHeaderAsync()
    {
        var ctx = httpContext.HttpContext;
        string? claim = ctx?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var id = Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;
        if (id == Guid.Empty)
            throw new ServiceException(StatusCodes.Status401Unauthorized, "Unauthorized");

        logger.LogInformation("Request as user: {user}", claim);
        _client.DefaultRequestHeaders.Add("X-WEBAUTH-USER", claim);
    }

    public Task<bool> AddCollaborator(Guid entityId, Guid userId)
    {
        SetAuthorizationHeaderAsync();

        throw new NotImplementedException();
    }


    public async Task<string> GetFile(string GitNamespace, string Path, string Branch = "main")
    {
        SetAuthorizationHeaderAsync();

        string? sha = null;
        var getFileResponse = await _client.GetAsync($"/api/v1/repos/{GitNamespace}/contents/{Path}");
        if (getFileResponse.IsSuccessStatusCode)
        {
            var existingFile = await getFileResponse.Content.ReadFromJsonAsync<ContentInfo>();
            sha = existingFile?.Sha;
        }

        var fileContent = new FileContentRequest
        {
            Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
            Message = $"U",
            Branch = branch,
            Sha = sha,
            Author = new GitUserInfo
            {
                Name = "NXT System",
                Email = "nxt@system.local"
            }
        };

        var response = await _httpClient.PutAsync(
            $"/api/v1/repos/{owner}/{repo}/contents/{path}",
            new StringContent(
                JsonSerializer.Serialize(fileContent),
                Encoding.UTF8,
                "application/json"
            )
        );

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FileContentResponse>()
            ?? throw new InvalidOperationException("Failed to upsert file");

    }

    public Task<(Git?, User?)> IsCollaborator(Guid entityId, Guid userId)
    {
        SetAuthorizationHeaderAsync();

        throw new NotImplementedException();
    }

    public Task<bool> RemoveCollaborator(Guid entityId, Guid userId)
    {
        SetAuthorizationHeaderAsync();

        throw new NotImplementedException();
    }

    public Task SetFile(string GitNamespace, string Path, string Content, string CommitMessage, string Branch = "main")
    {
        SetAuthorizationHeaderAsync();

        throw new NotImplementedException();
    }

    public Task<Git> UpdateRepository(string GitNamespace, GitRepoPatchRequestDTO DTO)
    {
        SetAuthorizationHeaderAsync();

        throw new NotImplementedException();
    }

	public async Task<Git> CreateRepository(GitRepoPostRequestDTO DTO, OwnerKind OwnerType)
	{
        SetAuthorizationHeaderAsync();

        var response = await _client.PostAsJsonAsync($"/api/v1/repos/{_gitTemplate}/generate", DTO);
        logger.LogInformation("Response: {response}", response);

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

	public async Task DeleteRepository(string GitNamespace)
	{
		throw new NotImplementedException();
	}

	// public async Task<Git> CreateRemoteRepository(string repositoryName, string? description = null)
	// {
	//     await SetAuthorizationHeaderAsync();
	//     var createOption = new CreateRepoOption
	//     {
	//         Name = repositoryName,
	//         Description = description,
	//         Private = false,
	//         AutoInit = true
	//     };

	//     var response = await _httpClient.PostAsJsonAsync("/api/v1/user/repos", createOption);
	//     _logger.LogInformation("Response: {response}", response);
	//     response.EnsureSuccessStatusCode();

	//     var repoResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

	//     // Map the response to our Git entity
	//     var git = new Git
	//     {
	//         Name = repoResponse.GetProperty("name").GetString() ?? string.Empty,
	//         Namespace = repoResponse.GetProperty("full_name").GetString() ?? string.Empty,
	//         Url = repoResponse.GetProperty("clone_url").GetString() ?? string.Empty,
	//         Branch = repoResponse.GetProperty("default_branch").GetString() ?? "main",
	//         ProviderType = GitProviderKind.Managed,
	//         OwnerType = GitOwnerKind.User // TODO: Once we support organizations within the app, this needs to be configurable
	//     };

	//     return await CreateAsync(git);
	// }

	// public async Task ArchiveRepository(string owner, string repo)
	// {
	//     await SetAuthorizationHeaderAsync();
	//     var editOption = new EditRepoOption
	//     {
	//         Archived = true
	//     };

	//     var response = await _httpClient.PatchAsync($"/api/v1/repos/{owner}/{repo}", JsonContent.Create(editOption));
	//     response.EnsureSuccessStatusCode();
	// }

	// public async Task<FileContentResponse> UpsertFile(
	//     string owner,
	//     string repo,
	//     string path,
	//     string content,
	//     string message,
	//     string branch = "main")
	// {
	//     await SetAuthorizationHeaderAsync();

	//     // First try to get the file to check if it exists
	//     var getFileResponse = await _httpClient.GetAsync($"/api/v1/repos/{owner}/{repo}/contents/{path}");
	//     string? sha = null;

	//     if (getFileResponse.IsSuccessStatusCode)
	//     {
	//         var existingFile = await getFileResponse.Content.ReadFromJsonAsync<ContentInfo>();
	//         sha = existingFile?.Sha;
	//     }

	//     var fileContent = new FileContentRequest
	//     {
	//         Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
	//         Message = message,
	//         Branch = branch,
	//         Sha = sha,
	//         Author = new GitUserInfo
	//         {
	//             Name = "NXT System",
	//             Email = "nxt@system.local"
	//         }
	//     };

	//     var response = await _httpClient.PutAsync(
	//         $"/api/v1/repos/{owner}/{repo}/contents/{path}",
	//         new StringContent(
	//             JsonSerializer.Serialize(fileContent),
	//             Encoding.UTF8,
	//             "application/json"
	//         )
	//     );

	//     response.EnsureSuccessStatusCode();
	//     return await response.Content.ReadFromJsonAsync<FileContentResponse>()
	//         ?? throw new InvalidOperationException("Failed to upsert file");
	// }

	// public async Task<string> GetRawFileContent(
	//     string name,
	//     string path,
	//     string branch = "main")
	// {
	//     await SetAuthorizationHeaderAsync();

	//     var requestUri = $"/api/v1/repos/{name}/raw/{path}?ref={branch}";
	//     var response = await _httpClient.GetAsync(requestUri);
	//     if (response.StatusCode == HttpStatusCode.NotFound)
	//         throw new ServiceException(StatusCodes.Status404NotFound, "File not found");
	//     response.EnsureSuccessStatusCode();
	//     return await response.Content.ReadAsStringAsync();
	// }
}
