// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses;

public class UserDetailsDO : BaseDO<Details>
{
    /// <summary>
    ///
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Bio { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string GithubUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LinkedinUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TwitterUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string WebsiteUrl { get; set; }

    public UserDetailsDO(Details details) : base(details)
    {
        Email = details.Email;
        Bio = details.Bio;
        FirstName = details.FirstName;
        LastName = details.LastName;
        GithubUrl = details.GithubUrl;
        LinkedinUrl = details.LinkedinUrl;
        TwitterUrl = details.TwitterUrl;
        WebsiteUrl = details.WebsiteUrl;
    }

    public static implicit operator UserDetailsDO?(Details? data) => data is null ? null : new(data);
}
