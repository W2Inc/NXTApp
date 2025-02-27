// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class MemberDO : BaseObjectDO<Member>
{
    public MemberDO(Member member) : base(member)
    {
        State = member.State;
        User = member.User;
        UserProjectId = member.UserProjectId;
    }

    /// <summary>
    /// The state of the invite.
    /// </summary>
    [Required]
    MemberInviteState State { get; set; }

    [Required]
    MinimalUserDTO? User { get; set; } // Assuming UserDO exists for the User class

    /// <summary>
    /// The user project this member is part of.
    /// </summary>
    [Required]
    Guid UserProjectId { get; set; }


    public static implicit operator MemberDO?(Member? entity) => entity is null ? null : new MemberDO(entity);
}
