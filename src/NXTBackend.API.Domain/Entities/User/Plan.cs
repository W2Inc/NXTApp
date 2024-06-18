﻿// ============================================================================
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