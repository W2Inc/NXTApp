using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests.Auth;

public class AuthRequestDto : BaseRequestDto
{
    [Required]
    [EmailAddress]
    // [StringLength(maximumLength: 10, MinimumLength = 5)]
    public string Email { get; set; }

    [Required]
    // [StringLength(maximumLength: 10, MinimumLength = 5)]
    public string Password { get; set; }
}