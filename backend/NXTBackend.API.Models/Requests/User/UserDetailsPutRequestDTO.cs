// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Models.Validators;

// ============================================================================

namespace NXTBackend.API.Models.Requests.User;

/// <summary>
/// Put request to set details regarding a user.
/// </summary>
public class UserDetailsPutRequestDTO : BaseRequestDTO
{
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public string? Email { get; set; }

    [StringLength(4096, ErrorMessage = "Bio cannot exceed 4096 characters")]
    public string? Markdown { get; set; }

    [StringLength(40, ErrorMessage = "Name cannot exceed 40 characters")]
    [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Contains invalid characters")]
    public string? FirstName { get; set; }

    [StringLength(40, ErrorMessage = "Name cannot exceed 40 characters")]
    [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Contains invalid characters")]
    public string? LastName { get; set; }

    [RegularExpression(@"^https://github\.com/[\w-]+(/[\w.-]+)*/?$",
        ErrorMessage = "GitHub URL must be a valid github.com profile URL")]
    public string? GithubUrl { get; set; }

    [RegularExpression(@"^https://(www\.)?linkedin\.com/in/[\w-]+/?$",
        ErrorMessage = "LinkedIn URL must be a valid linkedin.com/in profile URL")]
    public string? LinkedinUrl { get; set; }

    [RegularExpression(@"^https://(www\.)?(twitter|x)\.com/[\w-]+/?$",
        ErrorMessage = "Twitter/X URL must be a valid profile URL")]
    public string? TwitterUrl { get; set; }

    [RegularExpression(@"^https://.*$",
        ErrorMessage = "Website URL must start with https://")]
    public string? WebsiteUrl { get; set; }
}
