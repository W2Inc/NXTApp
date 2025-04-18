// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses;

public class UserDetailsDO : BaseObjectDO<Details>
{
    public UserDetailsDO(Details details) : base(details)
    {
        Email = details.Email;
        Markdown = details.Markdown;
        FirstName = details.FirstName;
        LastName = details.LastName;
        GithubUrl = details.GithubUrl;
        LinkedinUrl = details.LinkedinUrl;
        RedditUrl = details.RedditUrl;
        WebsiteUrl = details.WebsiteUrl;
    }

    public static implicit operator UserDetailsDO?(Details? data) => data is null ? null : new(data);

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? Email { get; set; }

	/// <summary>
	///
	/// </summary>
    [Required]
    public string? Markdown { get; set; }

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
    public string? RedditUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string? WebsiteUrl { get; set; }
}
