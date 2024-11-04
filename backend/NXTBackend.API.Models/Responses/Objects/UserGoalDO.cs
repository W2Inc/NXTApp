// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class UserGoalDO : BaseDO<UserGoal>
{
    public UserGoalDO(UserGoal userGoal) : base(userGoal)
    {
        User = userGoal.User;
        GoalId = userGoal.GoalId;
        UserCursusId = userGoal.UserCursusId;
        Members = userGoal.Members.Select(c => new MemberDO(c));
    }


    public virtual UserDO? User { get; set; }

    public Guid GoalId { get; set; }

    public Guid? UserCursusId { get; set; }

    public IEnumerable<MemberDO> Members { get; set; }

    public static implicit operator UserGoalDO?(UserGoal? entity) => entity is null ? null : new UserGoalDO(entity);
}
