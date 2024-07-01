using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Responses;

/// <summary>
/// Request for setting up a git repository
/// </summary>
public class GitInfoResponseDto : BaseRequestDto
{
    /// <summary>
    /// The URL to the git repository
    /// </summary>
    /// <example>
    /// https://github.com/W2Wizard/NXTBackend
    /// </example>
    [JsonPropertyName(nameof(GitUrl))]
    public string GitUrl { get; set; } = string.Empty;

    /// <summary>
    /// If null, will use the master branch
    /// </summary>
    public string GitBranch { get; set; } = string.Empty;

    /// <summary>
    /// If null, will use the latest commit
    /// </summary>
    public string GitCommit { get; set; } = string.Empty;
}