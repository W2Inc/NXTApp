// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities;

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_user")]
public class User : BaseEntity
{
    public User()
    {
        Login = string.Empty;
        DisplayName = string.Empty;
        AvatarUrl = string.Empty;
        Role = Role.Default;
        DetailsId = null;
        Details = null;
    }

    [Column("login")]
    public string Login { get; set; }

    [Column("display_name")]
    public string? DisplayName { get; set; }

    [Column("avatar_url"), Url]
    public string? AvatarUrl { get; set; }

    [Column("details_id")]
    public Guid? DetailsId { get; set; }

    [ForeignKey(nameof(DetailsId))]
    public virtual Details? Details { get; set; }

    [Column("role"), EnumDataType(typeof(Role))]
    public Role Role { get; set; }
}