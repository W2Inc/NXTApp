using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Shared;

public class GitVfsNodeDO
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// What encoding is used (most common is base64)
    /// </summary>
    [JsonPropertyName("encoding")]
    public string Encoding { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// URL to the remote for that file, can be used as a placeholder /
    /// provided to an IFrame for example etc.
    /// </summary>
    [JsonPropertyName("download_url")]
    public string Raw { get; set; }

    /// <summary>
    /// The actual file content encoded in base64.
    /// It's up to the frontend then to make out what do to with this.
    /// </summary>
	[JsonPropertyName("content")]
    public string? Content { get; set; }
}
