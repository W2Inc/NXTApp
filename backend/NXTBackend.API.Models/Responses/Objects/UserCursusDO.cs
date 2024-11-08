// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

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

    public TaskState State { get; set; }

    public Guid UserId { get; set; }

    public CursusDO? Cursus { get; set; }

    public static implicit operator UserCursusDO?(UserCursus? entity) => entity is null ? null : new(entity);
}
