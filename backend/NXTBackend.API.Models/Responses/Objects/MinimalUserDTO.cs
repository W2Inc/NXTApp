// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

/// <summary>
/// Minimal representation of a user.
/// </summary>
public class MinimalUserDTO : BaseObjectDO<User>
{
    public MinimalUserDTO(User user) : base(user)
    {
        Login = user.Login;
        DisplayName = user.DisplayName;
        AvatarUrl = user.AvatarUrl;
        DetailsId = user.DetailsId;
    }

    public static implicit operator MinimalUserDTO?(User? user) => user is null ? null : new(user);

    public string Login { get; set; }

    public string? DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public Guid? DetailsId { get; set; }
}
