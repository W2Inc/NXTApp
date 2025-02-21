// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXTBackend.API.Domain.Entities;

[Table("tbl_notifications")]
public class Notification : BaseEntity
{
    public Notification()
    {
        Message = string.Empty;
        Kind = NotificationKind.Default;
        UserNotifications = new HashSet<UserNotification>();
    }

    /// <summary>
    /// The notification message
    /// </summary>
    [Column("message")]
    public string Message { get; set; }

    /// <summary>
    /// The type of notification
    /// </summary>
    [Column("kind")]
    public NotificationKind Kind { get; set; }

    /// <summary>
    /// Optional reference to a resource this notification is about
    /// </summary>
    [Column("resource_id")]
    public Guid? ResourceId { get; set; }

    /// <summary>
    /// Collection of user-specific notification states
    /// </summary>
    public virtual ICollection<UserNotification> UserNotifications { get; set; }
}
