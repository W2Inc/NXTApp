using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Users;

[Table("tbl_user_details")]
public class Details : BaseEntity
{
    public Details()
    {
        Email = null;
        Bio = null;
        FirstName = null;
        LastName = null;
        GithubUrl = null;
        LinkedinUrl = null;
        TwitterUrl = null;
        WebsiteUrl = null;
        User = null!;
    }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("bio"), MaxLength(16384)]
    public string? Bio { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("github_url")]
    public string? GithubUrl { get; set; }

    [Column("linkedin_url")]
    public string? LinkedinUrl { get; set; }

    [Column("twitter_url")]
    public string? TwitterUrl { get; set; }

    [Column("website_url")]
    public string? WebsiteUrl { get; set; }
}
