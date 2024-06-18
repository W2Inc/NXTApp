using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Requests;

public class SearchRequestDto : BaseRequestDto
{
    [Required]
    public string Query { get; set; } = string.Empty;
}