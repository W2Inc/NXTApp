// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class UserDO(User user) : BaseDO<User>(user)
{
    public string Login { get; set; } = user.Login;

    public string? DisplayName { get; set; } = user.DisplayName;

    public string? AvatarUrl { get; set; } = user.AvatarUrl;

    public virtual UserDetailsDO? DetailsId { get; set; } = user.Details;

    /// <summary>
    /// Convert a possible user to a DO.
    /// </summary>
    /// <param name="user">The user.</param>
    public static implicit operator UserDO?(User? user) => user is null ? null : new(user);
}
