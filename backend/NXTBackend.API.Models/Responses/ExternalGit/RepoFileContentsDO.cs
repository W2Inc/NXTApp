using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Requests.ExternalGit;

public class RepoFileContentsDO
{
    [JsonPropertyName("content"), Base64String]
    public string Content { get; set; }

    [JsonPropertyName("sha")]
    public string Sha { get; set; }
}
