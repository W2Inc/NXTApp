using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Requests.Git;

public class GitRepoPatchRequestDTO : BaseRequestDTO
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("archive")]
    public bool? Archive { get; set; }
}
