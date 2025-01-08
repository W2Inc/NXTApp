// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Responses.Objects;

public class UserCursusDO : BaseObjectDO<UserCursus>
{
    public UserCursusDO(UserCursus userCursus) : base(userCursus)
    {
        State = userCursus.State;
        UserId = userCursus.UserId;
        Cursus = userCursus.Cursus;
    }

    [Required]
    public TaskState State { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public CursusDO? Cursus { get; set; }

    public static implicit operator UserCursusDO?(UserCursus? entity) => entity is null ? null : new(entity);
}
