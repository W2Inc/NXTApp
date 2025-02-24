// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Domain.Services.Impl;

// NOTE: BaseService implements BASIC CRUD functions.
/// <inheritdoc/>
public class NotificationService : BaseService<Notification>, INotificationService
{
    public NotificationService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("user_id", (q, userId) => q.Where(n =>
            // Include system notifications that haven't been marked as read
            (n.Kind == NotificationKind.System && !n.UserNotifications.Any(un => un.UserId == userId)) ||
            // Include user-specific notifications
            (n.Kind != NotificationKind.System && n.UserNotifications.Any(un => un.UserId == userId))
        ));

        DefineFilter<NotificationKind>("type", (q, type) => q.Where(p => p.Kind == type));
        DefineFilter<NotificationState>("state", (q, state) => q.Where(n =>
            // For system notifications, include those without a UserNotification (unread)
            // or those with matching state
            (n.Kind == NotificationKind.System &&
                ((state == NotificationState.None && !n.UserNotifications.Any()) ||
                 n.UserNotifications.Any(un => un.Status == state))) ||
            // For user notifications, just check the state
            (n.Kind != NotificationKind.System &&
                n.UserNotifications.Any(un => un.Status == state))
        ));
    }

    public override async Task<PaginatedList<Notification>> GetAllAsync(PaginationParams pagination, SortingParams sorting, FilterDictionary? filters = null)
    {
        var query = ApplyFilters(_dbSet.AsQueryable(), filters);
        if (filters?.ContainsKey("user_id") ?? false)
        {
            filters.TryGetValue("user_id", out object? userIdObj);
            var userId = userIdObj as Guid? ?? Guid.Empty;
            filters.TryGetValue("state", out object? stateObj); // Cast as NotificationState
            var state = stateObj as NotificationState?;

            // Add system notifications that haven't been marked as read/unread
            query = query.Union(
                _dbSet.Where(n =>
                    n.Kind == NotificationKind.System &&
                    (state == null ||
                     (state == NotificationState.None && !n.UserNotifications.Any(un => un.UserId == userId)) ||
                     n.UserNotifications.Any(un => un.UserId == userId && un.Status == state))
                )
            );
        }

        return await PaginatedList<Notification>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc/>
    public async Task<Notification> SendNotificationRangeAsync(IEnumerable<Guid> userIds, string message, NotificationKind kind = NotificationKind.Default, Guid? resourceId = null)
    {
        // Validate users exist
        var users = await _context.Users.AsNoTracking().Where(u => userIds.Contains(u.Id)).ToListAsync();
        if (users.Count == 0)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "No valid users found");

        var notification = new Notification
        {
            Message = message,
            Kind = kind,
            ResourceId = resourceId
        };

        var userNotifications = users.Select(user => new UserNotification
        {
            UserId = user.Id,
            Notification = notification,
            Status = NotificationState.None
        });

        await _context.Notifications.AddAsync(notification);
        await _context.UserNotifications.AddRangeAsync(userNotifications);
        await _context.SaveChangesAsync();
        return notification;
    }

    /// <inheritdoc/>
    public async Task<Notification> SendInviteNotificationRangeAsync(IEnumerable<Guid> userIds, string message, Guid userProjectId)
    {
        var userProject = await _context.UserProject.AsNoTracking().FirstAsync(up => up.Id == userProjectId);
        return await SendNotificationRangeAsync(userIds, message, NotificationKind.Invite, userProject.Id);
    }


    /// <inheritdoc/>
    public async Task MarkAsReadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null)
    {
        var now = DateTimeOffset.UtcNow;
        bool hasNotificationIds = notificationIds is not null && notificationIds.Any();
        var notificationsQuery = _context.Notifications.AsQueryable();
        if (hasNotificationIds)
        {
            // Apply filter if specific notifications are requested
            notificationsQuery = notificationsQuery.Where(n => notificationIds!.Contains(n.Id));
        }
        else
        {
            // For "mark all", we only need to create UserNotifications for system notifications
            notificationsQuery = notificationsQuery.Where(n => n.Kind == NotificationKind.System);
        }

        // Create UserNotifications for notifications that don't have one yet
        await _context.UserNotifications
            .AddRangeAsync(
                from n in notificationsQuery
                where !_context.UserNotifications.Any(un =>
                    un.NotificationId == n.Id &&
                    un.UserId == userId)
                select new UserNotification
                {
                    UserId = userId,
                    NotificationId = n.Id,
                    Status = NotificationState.Read,
                    ReadAt = now
                }
            );

        var updateQuery = _context.UserNotifications
            .Where(un => un.UserId == userId)
            .Where(un => un.Status != NotificationState.Read);

        if (hasNotificationIds)
            updateQuery = updateQuery.Where(un => notificationIds!.Contains(un.NotificationId));

        // Update existing notifications to read
        await updateQuery.ExecuteUpdateAsync(s => s
            .SetProperty(un => un.Status, NotificationState.Read)
            .SetProperty(un => un.ReadAt, now)
            .SetProperty(un => un.UpdatedAt, now));
    }

    /// <inheritdoc/>
    public async Task MarkAsUnreadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null)
    {
        var now = DateTimeOffset.UtcNow;
        var updateQuery = _context.UserNotifications
            .Where(un => un.UserId == userId)
            .Where(un => un.Status == NotificationState.Read);

        if (notificationIds is not null && notificationIds.Any())
            updateQuery = updateQuery.Where(un => notificationIds.Contains(un.NotificationId));

        // Update existing notifications to unread
        // Note: We only update Status, leaving ReadAt unchanged to preserve read history
        await updateQuery.ExecuteUpdateAsync(s => s.SetProperty(un => un.Status, NotificationState.None));
    }
}
