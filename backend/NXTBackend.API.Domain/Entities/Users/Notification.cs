// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXTBackend.API.Domain.Entities.Users;

[Table("tbl_user_notifications")]
public class UserNotification : BaseEntity
{
    public UserNotification()
    {
        Status = NotificationState.None;
        User = null!;
        Notification = null!;
    }

    /// <summary>
    /// The user this notification state belongs to
    /// </summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Reference to the user
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// The notification this state belongs to
    /// </summary>
    [Column("notification_id")]
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Reference to the notification
    /// </summary>
    [ForeignKey(nameof(NotificationId))]
    public virtual Notification Notification { get; set; }

    /// <summary>
    /// Current status of the notification for this user
    /// </summary>
    [Column("status")]
    public NotificationState Status { get; set; }

    /// <summary>
    /// When the notification was read by the user
    /// </summary>
    [Column("read_at")]
    public DateTimeOffset? ReadAt { get; set; }
}
