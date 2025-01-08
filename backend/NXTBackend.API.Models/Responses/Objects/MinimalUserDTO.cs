// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
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

    [Required]
    public string Login { get; set; }

    [Required]
    public string? DisplayName { get; set; }

    [Required]
    public string? AvatarUrl { get; set; }

    [Required]
    public Guid? DetailsId { get; set; }
}
