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
    Task<Notification> SendNotificationRangeAsync(
        IEnumerable<Guid> userIds,
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
    Task<Notification> SendInviteNotificationRangeAsync(IEnumerable<Guid> userIds, string message, Guid resourceId);

    /// <summary>
    /// Marks notifications as read for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="notificationIds">Optional list of specific notification IDs to mark as read. If null or empty, marks all notifications as read.</param>
    Task MarkAsReadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null);

    /// <summary>
    /// Marks notifications as unread for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="notificationIds">Optional list of specific notification IDs to mark as unread. If null or empty, marks all notifications as unread.</param>
    /// <remarks>
    /// This operation preserves the ReadAt timestamp to maintain read history, only updating the Status to unread.
    /// </remarks>
    Task MarkAsUnreadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null);
}
