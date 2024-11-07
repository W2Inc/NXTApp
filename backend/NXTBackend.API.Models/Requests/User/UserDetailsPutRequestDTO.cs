// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.User;

/// <summary>
/// Put request to set details regarding a user.
/// </summary>
public class UserDetailsPutRequestDTO : BaseRequestDTO
{
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(1024)]
    public string? Bio { get; set; }

    [DisplayName]
    public string? FirstName { get; set; }

    [DisplayName]
    public string? LastName { get; set; }

    [Url]
    public string? GithubUrl { get; set; }

    [Url]
    public string? LinkedinUrl { get; set; }

    [Url]
    public string? TwitterUrl { get; set; }

    [Url]
    public string? WebsiteUrl { get; set; }
}
