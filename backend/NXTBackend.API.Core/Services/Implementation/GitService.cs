using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class GitService : BaseService<Git>, IGitService
{
    private readonly HttpClient _httpClient;

    public GitService(DatabaseContext ctx, IHttpClientFactory clientFactory) : base(ctx)
    {
        _httpClient = clientFactory.CreateClient("NXTGit");
    }

    public async Task<string> GetCurrentUserAsync()
    {
        var response = await _httpClient.GetAsync("api/v1/user");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public Task<Git> CreateRemoteRepository(string repositoryName, string? description = null)
    {
        throw new NotImplementedException();
    }
}
