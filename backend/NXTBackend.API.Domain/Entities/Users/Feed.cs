// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities;

/// <summary>
/// Feed setting containing the mask of feeds that a user wants and doesn't want.
/// </summary>
[Table("tbl_user_feed")]
public class UserFeed : BaseEntity
{
    public UserFeed()
    {
        UserId = Guid.Empty;
    }

    /// <summary>
    /// The feed types this user has enabled
    /// </summary>
    [Column("visible_feed")]
    public FeedKind VisibleFeeds { get; set; }

    /// <summary>
    /// The ID of the user this configuration belongs to
    /// </summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// The user this configuration belongs to
    /// </summary>
    public virtual User User { get; set; } = null!;
}
