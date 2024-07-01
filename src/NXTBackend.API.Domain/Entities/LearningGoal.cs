// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.User;

namespace NXTBackend.API.Domain.Entities;

/*
model LearningGoal {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    name        String @unique
    slug        String @unique
    markdown    String
    description String @db.VarChar(256)

    public     Boolean @default(false)
    enabled    Boolean @default(false)
    deprecated Boolean @default(false)

    owner_id String @db.Uuid
    owner    User   @relation("GoalCreator", fields: [owner_id], references: [id])

    projects      Project[]
    user_goals    UserGoal[]
    goal_vertices CursusVertex[] // This goal is part of these vertices

    @@map("learning_goal")
}
*/

[Table("tbl_learning_goal")]
public class LearningGoal : BaseEntity
{
    public LearningGoal()
    {
        Name = string.Empty;
        Slug = string.Empty;
        Markdown = string.Empty;
        Description = string.Empty;
        CreatorId = Guid.Empty;
        Creator = null!;
        Projects = [];
        UserGoals = [];
        GoalVertices = [];
    }

    [Column("name")]
    public string Name { get; set; }

    [Column("slug")]
    public string Slug { get; set; }

    [Column("markdown")]
    public string Markdown { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public virtual User Creator { get; set; }

    /// <summary>
    /// The projects that are part of this goal
    /// </summary>
    public virtual ICollection<Project> Projects { get; set; }

    /// <summary>
    /// The instances of this goal
    /// </summary>
    public virtual ICollection<UserGoal> UserGoals { get; set; }

    /// <summary>
    /// This goal is part of these vertices
    /// </summary>
    public virtual ICollection<CursusVertex> GoalVertices { get; set; }
}