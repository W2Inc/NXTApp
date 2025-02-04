using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace MyAspNetApp;

public class GiteaService
{
    private readonly HttpClient _httpClient;
    private readonly string _serverUrl;

    public GiteaService(IOptions<GiteaOptions> giteaOptions)
    {
        _httpClient = new HttpClient();

        // Use the "token" or "Bearer" scheme depending on your configuration:
        // For Gitea personal access tokens, "token" is often used:
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("token", giteaOptions.Value.PersonalAccessToken);

        _serverUrl = giteaOptions.Value.ServerUrl.TrimEnd('/');
    }

    public async Task<string> GetCurrentUserAsync()
    {
        var url = $"{_serverUrl}/api/v1/user";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    // Example: create more methods to interact with other Gitea endpoints
}
