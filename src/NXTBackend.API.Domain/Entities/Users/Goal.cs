// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Users;

/*
model UserGoal {
    id         String       @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime     @default(now())
    updated_at DateTime     @default(now()) @updatedAt
    state      State        @default(INACTIVE)
    user       User         @relation(fields: [user_id], references: [id])
    goal       LearningGoal @relation(fields: [goal_id], references: [id])

    user_id String @db.Uuid
    goal_id String @db.Uuid

    project_users ProjectMember[]

    user_cursus_id String?     @db.Uuid
    user_cursus    UserCursus? @relation("UserGoalsInCursus", fields: [user_cursus_id], references: [id])

    vertex_id String       @db.Uuid
    vertex    CursusVertex @relation(fields: [vertex_id], references: [id])

    @@map("user_goal")
}
*/

[Table("tbl_user_goal")]
public class UserGoal : BaseEntity
{
    public UserGoal()
    {
        UserId = Guid.Empty;
        User = null!;

        GoalId = Guid.Empty;
        Goal = null!;

        UserCursusId = null;
        UserCursus = null;

        VertexId = Guid.Empty;
        Vertex = null!;
    }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    [Column("goal_id")]
    public Guid GoalId { get; set; }

    [ForeignKey(nameof(GoalId))]
    public virtual LearningGoal Goal { get; set; }

    [Column("user_cursus_id")]
    public Guid? UserCursusId { get; set; }

    [ForeignKey(nameof(UserCursusId))]
    public virtual UserCursus? UserCursus { get; set; }

    [Column("vertex_id")]
    public Guid VertexId { get; set; }

    [ForeignKey(nameof(VertexId))]
    public virtual CursusVertex Vertex { get; set; }

    /// <summary>
    /// All the members that are part of this project.
    /// At most N members to what the project allows.
    /// </summary>
    public virtual ICollection<Member> Members { get; set; } = [];
}