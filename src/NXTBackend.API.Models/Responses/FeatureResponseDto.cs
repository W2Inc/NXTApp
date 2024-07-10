using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Responses;

/// <summary>
/// Request for setting up a git repository
/// </summary>
public class FeatureResponseDto : BaseResponseDto
{
    /// <summary>
    /// The URL to the git repository
    /// </summary>
    /// <example>
    /// https://github.com/W2Wizard/NXTBackend
    /// </example>
    [JsonPropertyName(nameof(Name))]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// If null, will use the master branch
    /// </summary>
    [JsonPropertyName(nameof(IsPublic))]

    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// If null, will use the latest commit
    /// </summary>
    [JsonPropertyName(nameof(Markdown))]
    public string Markdown { get; set; } = string.Empty;
}