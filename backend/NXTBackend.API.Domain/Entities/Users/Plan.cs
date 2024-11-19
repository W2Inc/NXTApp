// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Users;

/// <summary>
/// Table to keep track of the user's plan.
/// </summary>
[Table("tbl_user_plan")]
public class Plan : BaseEntity
{
    public Plan()
    {
        Kind = PlanKind.Free;
        ExpiresAt = DateTimeOffset.UtcNow;
        UserId = null;
        User = null;
    }

    [Column("login")]
    public PlanKind Kind { get; set; }

    [Column("expires_at")]
    public DateTimeOffset? ExpiresAt { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}
