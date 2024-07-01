using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests;

public class SearchRequestDto : BaseRequestDto
{
    [Required]
    public string Query { get; set; } = string.Empty;
}