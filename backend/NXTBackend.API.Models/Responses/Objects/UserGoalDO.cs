// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class UserGoalDO : BaseObjectDO<UserGoal>
{
    public UserGoalDO(UserGoal userGoal) : base(userGoal)
    {
        User = userGoal.User;
        GoalId = userGoal.GoalId;
        UserCursusId = userGoal.UserCursusId;
    }

    [Required]
    public virtual MinimalUserDTO? User { get; set; }

    [Required]
    public Guid GoalId { get; set; }

    [Required]
    public Guid? UserCursusId { get; set; }

    public static implicit operator UserGoalDO?(UserGoal? entity) => entity is null ? null : new UserGoalDO(entity);
}
