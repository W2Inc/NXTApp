// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Domain.Services.Impl;

/// <inheritdoc/>
public class NotificationService : BaseService<Notification>, INotificationService
{
    public NotificationService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<NotificationKind>("type", (q, type) => q.Where(p => p.Kind == type));
    }

    /// <inheritdoc/>
    public async Task<Notification> SendNotificationAsync(Guid userId, string message, NotificationKind kind = NotificationKind.Default, Guid? resourceId = null)
    {
        // Validate user exists
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new ServiceException(StatusCodes.Status422UnprocessableEntity, $"User with ID {userId} not found");

        var notification = new Notification
        {
            Message = message,
            Kind = kind,
            ResourceId = resourceId
        };

        var userNotification = new UserNotification
        {
            UserId = user.Id,
            Notification = notification,
            Status = NotificationState.None
        };

        await _context.Notifications.AddAsync(notification);
        await _context.UserNotifications.AddAsync(userNotification);
        await _context.SaveChangesAsync();
        return notification;
    }

    /// <inheritdoc/>
    public async Task<Notification> SendInviteNotificationAsync(Guid userId, string message, Guid userProjectId)
    {
        var userProject = await _context.UserProject.AsNoTracking().FirstAsync(up => up.Id == userProjectId);
        return await SendNotificationAsync(userId, message, NotificationKind.Invite, userProject.Id);
    }

    /// <inheritdoc/>
    public async Task MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var userNotification = await _context.UserNotifications
            .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notificationId)
            ?? throw new ServiceException(StatusCodes.Status422UnprocessableEntity, $"Notification {notificationId} not found for user {userId}");

        userNotification.Status = NotificationState.Read;
        userNotification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new ServiceException(StatusCodes.Status422UnprocessableEntity, $"User with ID {userId} not found");

        var now = DateTimeOffset.Now;
        await _context.UserNotifications
            .Where(un => un.UserId == userId && un.Status == NotificationState.None)
            .ExecuteUpdateAsync(s => s
                .SetProperty(un => un.Status, NotificationState.Read)
                .SetProperty(un => un.ReadAt, now));
    }
}
