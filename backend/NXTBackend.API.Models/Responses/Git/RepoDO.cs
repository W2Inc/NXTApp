using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Responses.Git;

public class RepoDO
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("clone_url")]
    public string CloneUrl { get; set; }

    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }
}
