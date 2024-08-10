// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Users;

/*
model UserCursus {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    state State @default(INACTIVE)

    user_id String @db.Uuid
    user    User   @relation("UserCursi", fields: [user_id], references: [id])

    cursus_id String @db.Uuid
    cursus    Cursus @relation(fields: [cursus_id], references: [id])

    // All the user goal instances that exist in that graph, this will
    // work together with the Goal being part of the CursusVertex
    user_goals UserGoal[] @relation("UserGoalsInCursus")

    @@map("user_cursus")
}
*/

/// <summary>
/// User projects are project "instances" / "sessions" for a project.
/// User's can partake in this session and complete it with the help of other members.
/// </summary>
[Table("tbl_user_cursus")]
public class UserCursus : BaseEntity
{
    public UserCursus()
    {
        State = TaskState.Inactive;
        UserId = Guid.Empty;
        User = null!;
        CursusId = Guid.Empty;
        Cursus = null!;
        UserGoals = [];
    }

    [Column("state")]
    public TaskState State { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    [Column("cursus_id")]
    public Guid CursusId { get; set; }

    [ForeignKey(nameof(CursusId))]
    public virtual Cursus Cursus { get; set; }

    /// <summary>
    /// All the members that are part of this project.
    /// At most N members to what the project allows.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<UserGoal> UserGoals { get; set; }
}