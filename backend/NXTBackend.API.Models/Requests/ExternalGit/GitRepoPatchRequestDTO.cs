using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Requests.ExternalGit;

public class GitRepoPatchRequestDTO : BaseRequestDTO
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("archived")]
    public bool? Archived { get; set; }
}
