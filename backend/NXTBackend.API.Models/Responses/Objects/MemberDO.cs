// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class MemberDO : BaseDO<Member>
{
    public MemberDO(Member member) : base(member)
    {
        State = member.State;
        User = member.User;
        UserGoalId = member.UserGoalId;
        UserProjectId = member.UserProjectId;
    }

    /// <summary>
    /// The state of the invite.
    /// </summary>
    public MemberInviteState State { get; set; }

    public virtual UserDO? User { get; set; } // Assuming UserDO exists for the User class

    /// <summary>
    /// Which user goal this member is working on.
    /// </summary>
    public Guid? UserGoalId { get; set; }

    /// <summary>
    /// The user project this member is part of.
    /// </summary>
    public Guid UserProjectId { get; set; }


    public static implicit operator MemberDO?(Member? entity) => entity is null ? null : new MemberDO(entity);
}
