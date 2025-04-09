// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Evaluation;

/*
model Rubric {
    id String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid

    name       String   @unique
    markdown   String
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt
    public     Boolean  @default(false)
    enabled    Boolean  @default(false)
    deprecated Boolean  @default(false)

    project_id String @unique @db.Uuid

    owner_id String @db.Uuid
    owner    User   @relation("RubricCreator", fields: [owner_id], references: [id])

    git_info_id String?  @db.Uuid
    git_info    GitInfo? @relation(fields: [git_info_id], references: [id])

    project       Project       @relation(fields: [project_id], references: [id])
    reviews       Review[]
    user_projects UserProject[]

    @@map("rubric")
}
*/

// ============================================================================

[Table("tbl_rubric")]
public class Rubric : BaseEntity
{
    public Rubric()
    {
        Name = string.Empty;
        Markdown = string.Empty;
        Public = false;
        Enabled = false;

        ProjectId = Guid.Empty;
        Project = null!;

        CreatorId = Guid.Empty;
        Creator = null!;

        GitInfoId = Guid.Empty;
        GitInfo = null!;
    }

    [Column("name")]
    public string Name { get; set; }

    [Column("markdown")]
    public string Markdown { get; set; }

    [Column("public")]
    public bool Public { get; set; }

    [Column("enabled")]
    public bool Enabled { get; set; }

    [Column("project_id")]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public virtual User Creator { get; set; }

    [Column("git_info_id")]
    public Guid GitInfoId { get; set; }

    [ForeignKey(nameof(GitInfoId))]
    public virtual Git GitInfo { get; set; }

    [JsonIgnore]
    public virtual IEnumerable<Review> Reviews { get; set; }

    [JsonIgnore]
    public virtual IEnumerable<UserProject> UserProjects { get; set; }
}
