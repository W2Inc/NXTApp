// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses;

public class UserDetailsDO : BaseObjectDO<Details>
{
    public UserDetailsDO(Details details) : base(details)
    {
        Email = details.Email;
        // Bio = details.Bio;
        FirstName = details.FirstName;
        LastName = details.LastName;
        GithubUrl = details.GithubUrl;
        LinkedinUrl = details.LinkedinUrl;
        TwitterUrl = details.TwitterUrl;
        WebsiteUrl = details.WebsiteUrl;
    }

    public static implicit operator UserDetailsDO?(Details? data) => data is null ? null : new(data);

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? Email { get; set; }

    /// <summary>
    /// NOTE: Not included in DO, use separate endpoint
    /// Why ? Some Markdown Biographies can be full of shit. We won't want that
    /// for EVERY single user we request.
    /// </summary>
    // public string? Bio { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? FirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? LastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? GithubUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? LinkedinUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? TwitterUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? WebsiteUrl { get; set; }
}
