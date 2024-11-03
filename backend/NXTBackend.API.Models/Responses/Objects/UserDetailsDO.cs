// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses;

public class UserDetailsDO(Details details) : BaseDO<Details>(details)
{
    public static implicit operator UserDetailsDO?(Details? data) => data is null ? null : new(data);

    /// <summary>
    ///
    /// </summary>
    public string Email { get; set; } = details.Email;

    /// <summary>
    ///
    /// </summary>
    public string Bio { get; set; } = details.Bio;

    /// <summary>
    ///
    /// </summary>
    public string FirstName { get; set; } = details.FirstName;

    /// <summary>
    ///
    /// </summary>
    public string LastName { get; set; } = details.LastName;

    /// <summary>
    ///
    /// </summary>
    public string GithubUrl { get; set; } = details.GithubUrl;

    /// <summary>
    ///
    /// </summary>
    public string LinkedinUrl { get; set; } = details.LinkedinUrl;

    /// <summary>
    ///
    /// </summary>
    public string TwitterUrl { get; set; } = details.TwitterUrl;

    /// <summary>
    ///
    /// </summary>
    public string WebsiteUrl { get; set; } = details.WebsiteUrl;
}
