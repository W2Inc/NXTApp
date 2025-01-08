// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class UserDO : BaseObjectDO<User>
{
    public UserDO(User user) : base(user)
    {
        Login = user.Login;
        DisplayName = user.DisplayName;
        AvatarUrl = user.AvatarUrl;
        Details = user.Details;
    }

    public static implicit operator UserDO?(User? user) => user is null ? null : new(user);

    [Required]
    public string Login { get; set; }

    [Required]
    public string? DisplayName { get; set; }

    [Required]
    public string? AvatarUrl { get; set; }

    [Required]
    public virtual UserDetailsDO? Details { get; set; }
}
