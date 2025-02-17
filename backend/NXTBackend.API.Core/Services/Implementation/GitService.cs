// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Responses.Gitea;

namespace NXTBackend.API.Core.Services;

public sealed class GitService : BaseService<Git>, IGitService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly ILogger<GitService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GitService(
        DatabaseContext ctx,
        IDistributedCache cache,
        IHttpClientFactory clientFactory,
        ILogger<GitService> logger,
        IHttpContextAccessor httpContextAccessor) : base(ctx)
    {
        _httpClient = clientFactory.CreateClient("NXTGit");
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
        _logger = logger;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string? claim = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var id = Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;
        if (id == Guid.Empty)
            throw new ServiceException(StatusCodes.Status401Unauthorized, "Unauthorized");

        string cacheKey = $"USER_{guid}";
        string? cachedUser = await _cache.GetStringAsync(cacheKey);
        if (string.IsNullOrEmpty(cachedUser))
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "No login found");

        _logger.LogInformation("Request as user: {user}", cachedUser);
        _httpClient.DefaultRequestHeaders.Add("X-WEBAUTH-USER", cachedUser);
    }

    public async Task<Git> CreateRemoteRepository(string repositoryName, string? description = null)
    {
        await SetAuthorizationHeaderAsync();
        var createOption = new CreateRepoOption
        {
            Name = repositoryName,
            Description = description,
            Private = false,
            AutoInit = true
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/user/repos", createOption);
        _logger.LogInformation("Response: {response}", response);
        response.EnsureSuccessStatusCode();

        var repoResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Map the response to our Git entity
        var git = new Git
        {
            Name = repoResponse.GetProperty("name").GetString() ?? string.Empty,
            Namespace = repoResponse.GetProperty("full_name").GetString() ?? string.Empty,
            Url = repoResponse.GetProperty("clone_url").GetString() ?? string.Empty,
            Branch = repoResponse.GetProperty("default_branch").GetString() ?? "main",
            ProviderType = GitProviderKind.Managed,
            OwnerType = GitOwnerKind.User // TODO: Once we support organizations within the app, this needs to be configurable
        };

        return await CreateAsync(git);
    }

    public async Task ArchiveRepository(string owner, string repo)
    {
        await SetAuthorizationHeaderAsync();
        var editOption = new EditRepoOption
        {
            Archived = true
        };

        var response = await _httpClient.PatchAsync($"/api/v1/repos/{owner}/{repo}", JsonContent.Create(editOption));
        response.EnsureSuccessStatusCode();
    }

    public async Task<FileContentResponse> UpsertFile(
        string owner,
        string repo,
        string path,
        string content,
        string message,
        string branch = "main")
    {
        await SetAuthorizationHeaderAsync();

        // First try to get the file to check if it exists
        var getFileResponse = await _httpClient.GetAsync($"/api/v1/repos/{owner}/{repo}/contents/{path}");
        string? sha = null;

        if (getFileResponse.IsSuccessStatusCode)
        {
            var existingFile = await getFileResponse.Content.ReadFromJsonAsync<ContentInfo>();
            sha = existingFile?.Sha;
        }

        var fileContent = new FileContentRequest
        {
            Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
            Message = message,
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

        var result = await response.Content.ReadFromJsonAsync<FileContentResponse>();
        if (result == null)
            throw new InvalidOperationException("Failed to upsert file");

        return result;
    }
}
