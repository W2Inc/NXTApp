using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Shared;

public class GitVfsNodeDO
{
	[JsonPropertyName("path")]
    public string Path { get; set; }

	[JsonPropertyName("encoding")]
    public string Encoding { get; set; }

	[JsonPropertyName("download_url")]
    public string Raw { get; set; }

	[JsonPropertyName("content")]
    public string? Content { get; set; }
}