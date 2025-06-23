// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Domain.Entities;

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_comment")]
[Index(nameof(ParentId))]
[Index(nameof(UserId))]
public class Comment : BaseEntity
{
    /// <summary>
    /// NameOf what this comment is for.
    /// </summary>
    [Column("parent_type")]
    public string ParentType { get; set; }

    /// <summary>
    /// The resource this comment is targeted to.
    /// </summary>
    [Column("parent_id")]
    public Guid ParentId { get; set; }

    /// <summary>
    /// The message sent
    /// </summary>
    [Column("markdown")]
    public string Markdown { get; set; }

    /// <summary>
    /// The user who sent it.
    /// </summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}
