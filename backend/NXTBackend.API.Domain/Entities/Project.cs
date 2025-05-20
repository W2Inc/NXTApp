// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Domain.Entities;

/*
model Project {
    id String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid

    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    name          String  @unique
    description   String  @db.VarChar(256)
    markdown      String
    slug          String  @unique
    thumbnail_url String?
    public        Boolean @default(false)
    enabled       Boolean @default(false)
    deprecated    Boolean @default(false)
    max_members   Int     @default(1)

    // Objectives that are part of this project that get selected on review
    // For example: "Learn to use git", "Learn to use github", "Learn to use gitlab"
    //objectives    Objective[]
    rubrics       Rubric[]
    goals         LearningGoal[]
    user_projects UserProject[]

    git_info_id String?  @db.Uuid
    git_info    GitInfo? @relation(fields: [git_info_id], references: [id])

    owner_id String @db.Uuid
    owner    User   @relation("ProjectCreator", fields: [owner_id], references: [id])
    tags     Tag[]  @relation("ProjectTags")

    @@map("project")
}
*/

[Table("tbl_project")]
public class Project : BaseEntity
{
    public Project()
    {
        Name = string.Empty;
        Description = string.Empty;
        Slug = string.Empty;
        ThumbnailUrl = null;
        Deprecated = false;
        Public = false;
        Enabled = false;
        MaxMembers = 1;

        GitInfoId = Guid.Empty;
        GitInfo = null!;

        CreatorId = Guid.Empty;
        Creator = null!;
    }

    /// <summary>
    /// The name of the feature.
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// The markdown content of the feature.
    /// </summary>
    [Column("description")]
    public string Description { get; set; }

    [Column("slug")]
    public string Slug { get; set; }

    [Column("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [Column("public"), DefaultValue(false)]
    public bool Public { get; set; }

    [Column("enabled"), DefaultValue(false)]
    public bool Enabled { get; set; }

    [Column("deprecated"), DefaultValue(false)]
    public bool Deprecated { get; set; }

    [Column("max_members"), DefaultValue(3)]
    public int MaxMembers { get; set; }

    [Column("git_info_id")]
    public Guid GitInfoId { get; set; }

    [ForeignKey(nameof(GitInfoId))]
    public virtual Git GitInfo { get; set; }

    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public virtual User Creator { get; set; }

    [Column("tags")]
    public string[] Tags { get; set; }

    /// <summary>
    /// Rubrics that exist for this project.
    /// </summary>
    public virtual ICollection<Rubric> Rubrics { get; set; }

    /// <summary>
    /// The goals that reference this project
    /// </summary>
    public virtual ICollection<LearningGoal> Goals { get; set; }

    /// <summary>
    /// The different sessions that exist for this project
    /// </summary>
    public virtual ICollection<UserProject> UserProjects { get; set; }
}
