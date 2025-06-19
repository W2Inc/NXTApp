// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Evaluation;

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_comment")]
public class Comment : BaseEntity
{
    public Comment()
    {
        Markdown = string.Empty;
        UserId = Guid.Empty;
    }

    [Column("markdown")]
    public string Markdown { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}
