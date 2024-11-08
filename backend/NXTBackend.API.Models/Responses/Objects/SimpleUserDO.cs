// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class SimpleUserDO : BaseObjectDO<User>
{
    public SimpleUserDO(User user) : base(user)
    {
        Login = user.Login;
        DisplayName = user.DisplayName;
        AvatarUrl = user.AvatarUrl;
        DetailsId = user.DetailsId;
    }

    public static implicit operator SimpleUserDO?(User? user) => user is null ? null : new(user);

    public string Login { get; set; }

    public string? DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public Guid? DetailsId { get; set; }
}
