using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Users;

/*
 model UserDetails {
    id      String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    user_id String @unique @db.Uuid
    user    User   @relation(fields: [user_id], references: [id])

    email      String?  @unique
    bio        String?
    first_name String?
    last_name  String?
    features   String[] @default([]) @db.Uuid

    github_url   String?
    linkedin_url String?
    twitter_url  String?
    website_url  String?

    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    @@map("user_details")
}
 */

[Table("tbl_user_details")]
public class Details : BaseEntity
{
    public Details()
    {
        Email = string.Empty;
        Bio = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        GithubUrl = string.Empty;
        LinkedinUrl = string.Empty;
        TwitterUrl = string.Empty;
        WebsiteUrl = string.Empty;
    }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    [Column("email"), EmailAddress]
    public string Email { get; set; }

    [Column("bio"), StringLength(1024)]
    public string Bio { get; set; }

    [Column("first_name"), DisplayName]
    public string FirstName { get; set; }

    [Column("last_name"), DisplayName]
    public string LastName { get; set; }

    [Column("github_url"), Url]
    public string GithubUrl { get; set; }

    [Column("linkedin_url"), Url]
    public string LinkedinUrl { get; set; }

    [Column("twitter_url"), Url]
    public string TwitterUrl { get; set; }

    [Column("website_url"), Url]
    public string WebsiteUrl { get; set; }
}