// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

// ============================================================================

namespace NXTBackend.API.Domain.Entities;

[Table("tbl_notifications")]
public class Notifications : BaseEntity
{
    public Notifications()
    {
        Href = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Type = NotificationKind.Default;
    }

    /// <summary>
    /// The title of the event
    /// </summary>
    [Column("title")]
    public string Title { get; set; }

    /// <summary>
    /// The title of the event
    /// </summary>
    [Column("type")]
    public NotificationKind Type { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    /// The ID of which notification content.
    /// </summary>
    [Column("content_id")]
    public Guid ContentId { get; set; }

    [ForeignKey(nameof(ContentId))]
    public NotificationsContent Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("href")]
    public string Href { get; set; }
}
