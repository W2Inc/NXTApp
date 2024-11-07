using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests;

public class SearchRequestDTO : BaseRequestDTO
{
    [Required]
    public string Query { get; set; } = string.Empty;
}
