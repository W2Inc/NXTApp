// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.UserProject;

/*
model ProjectMember {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    // User is awaiting an invite to join the project as a active member
    is_pending_invite Boolean @default(true)

    user    User   @relation(fields: [user_id], references: [id])
    user_id String @db.Uuid

    user_goal_id String?   @db.Uuid
    user_goal    UserGoal? @relation(fields: [user_goal_id], references: [id])

    user_project_id String      @db.Uuid
    user_project    UserProject @relation(fields: [user_project_id], references: [id])

    @@map("project_member")
}
*/

[Table("tbl_user_project_member")]
public class Member : BaseEntity
{
    public Member()
    {
        State = MemberInviteState.Pending;
        UserId = Guid.Empty;
        User = null!;
        UserGoalId = null;
        UserGoal = null;
        UserProjectId = Guid.Empty;
        UserProject = null!;
    }

    /// <summary>
    /// The state of the invite. On Decline the row will be removed.
    /// </summary>
    [Column("invite_state")]
    public MemberInviteState State { get; set; }

    /// <summary>
    /// The user that is part of the project.
    /// </summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// Which user goal this member is working on.
    /// </summary>
    [Column("user_goal_id")]
    public Guid? UserGoalId { get; set; }

    [ForeignKey(nameof(UserGoalId))]
    public virtual UserGoal? UserGoal { get; set; }

    /// <summary>
    /// The user project this member is part of.
    /// 
    /// For users this "session" is shared amongst all members.
    /// </summary>
    [Column("user_project_id")]
    public Guid UserProjectId { get; set; }

    [ForeignKey(nameof(UserProjectId))]
    public virtual UserProject UserProject { get; set; }
}