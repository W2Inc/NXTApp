// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Responses.Objects;

public class UserNotificationDO : BaseObjectDO<UserNotification>
{
    public UserNotificationDO(UserNotification userNotification) : base(userNotification)
    {
        UserId = userNotification.UserId;
        Status = userNotification.Status;
        ReadAt = userNotification.ReadAt;
        Notification = userNotification.Notification!;
    }

    /// <summary>
    /// The ID of the user this notification belongs to
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The current status of the notification
    /// </summary>
    [Required]
    public NotificationState Status { get; set; }

    /// <summary>
    /// When the notification was read by the user
    /// </summary>
    public DateTimeOffset? ReadAt { get; set; }

    /// <summary>
    /// The notification details
    /// </summary>
    [Required]
    public NotificationDO Notification { get; set; }

    public static implicit operator UserNotificationDO?(UserNotification? entity) => entity is null ? null : new UserNotificationDO(entity);
}
