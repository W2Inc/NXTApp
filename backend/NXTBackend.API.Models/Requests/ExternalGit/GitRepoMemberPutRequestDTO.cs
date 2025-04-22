using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Requests.ExternalGit;

public class GitRepoMemberPostRequestDTO : BaseRequestDTO
{
    [Required, JsonPropertyName("permission")]
    public string Permission { get; set; } = "write";
}
