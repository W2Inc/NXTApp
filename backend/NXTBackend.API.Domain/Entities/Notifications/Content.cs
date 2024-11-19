// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Notification;

/// <summary>
/// The content of a notification
/// </summary>
[Table("tbl_notifications_content")]
public class NotificationsContent : BaseEntity
{
    public NotificationsContent()
    {
        Markdown = string.Empty;
        Hash = Markdown.GetHashCode();
    }

    /// <summary>
    /// The title of the event
    /// </summary>
    [Column("title")]
    public string Markdown { get; set; }

    /// <summary>
    /// Computed hash code from the markdown body to detect duplicates.
    /// </summary>
    [Column("hash")]
    public int Hash { get; set; }
}
