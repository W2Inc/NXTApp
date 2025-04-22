using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Requests.Git;

/// <summary>
/// Create a repository based on a template.
/// </summary>
public class GitRepoPostCommitRequestDTO : BaseRequestDTO
{
    [Required, JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("avatar")]
    public bool Avatar { get; set; } = true;

    [JsonPropertyName("default_branch")]
    public string? DefaultBranch { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }
}
