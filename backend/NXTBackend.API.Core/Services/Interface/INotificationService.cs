// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for managing notifications
/// </summary>
public interface INotificationService : IDomainService<Notification>
{
    /// <summary>
    /// Creates a notification for a user
    /// </summary>
    /// <param name="userId">The user to send the notification to</param>
    /// <param name="message">The notification message</param>
    /// <param name="kind">The type of notification</param>
    /// <param name="resourceId">Optional ID of the resource this notification is about</param>
    Task<Notification> SendNotificationAsync(
        Guid userId,
        string message,
        NotificationKind kind = NotificationKind.Default,
        Guid? resourceId = null
    );

    /// <summary>
    /// Creates an invite notification
    /// </summary>
    /// <param name="userId">The user to send the invite to</param>
    /// <param name="message">The invite message</param>
    /// <param name="resourceId">The ID of the resource being invited to</param>
    Task<Notification> SendInviteNotificationAsync(Guid userId, string message, Guid resourceId);

    /// <summary>
    /// Marks a notification as read for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="notificationId">The notification ID</param>
    Task MarkAsReadAsync(Guid userId, Guid notificationId);

    /// <summary>
    /// Marks a notification as read for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="notificationId">The notification ID</param>
    Task MarkAsReadAsync(Guid userId, IEnumerable<Guid> notificationIds);

    /// <summary>
    /// Marks all notifications as read for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    Task MarkAllAsReadAsync(Guid userId);


}
