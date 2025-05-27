// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

// NOTE: BaseService implements BASIC CRUD functions.
/// <inheritdoc/>
public class NotificationService : BaseService<Notification>, INotificationService
{
	public NotificationService(DatabaseContext ctx) : base(ctx)
	{
		DefineFilter<bool>("read", (q, read) => q.Where(n => read ? n.ReadAt != null : n.ReadAt == null));
		DefineFilter<Guid>("user_id", (q, userId) => q.Where(n => n.NotifiableId == userId));
		DefineFilter<NotificationState>("state", (q, state) => q.Where(n => n.State == state));
		DefineFilter<NotificationKind>("kind", (q, kind) => q.Where(n => (n.Descriptor & kind) != 0));
		DefineFilter<NotificationKind>("not[kind]", (q, kind) => q.Where(n => (n.Descriptor & kind) == 0));
	}

    public override async Task<PaginatedList<Notification>> GetAllAsync(PaginationParams pagination, SortingParams sorting, FilterDictionary? filters = null)
    {
        var query = ApplyFilters(_dbSet.AsQueryable(), filters);
        query = SortedList<Notification>.Apply(query, sorting);
        return await PaginatedList<Notification>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc/>
    public async Task MarkAsReadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null)
    {
        if (notificationIds is not null && notificationIds.Any())
        {
            var idList = notificationIds.ToList();
            bool hasUnauthorizedNotifications = await _context.Notifications.AnyAsync(n => idList.Contains(n.Id) && n.NotifiableId != userId);
            if (hasUnauthorizedNotifications)
                throw new ServiceException(StatusCodes.Status403Forbidden, "One or more notification IDs do not belong to the current user");
        }

        var updateQuery = _context.Notifications.Where(un => un.NotifiableId == userId);
        if (notificationIds is not null && notificationIds.Any())
            updateQuery = updateQuery.Where(un => notificationIds.Contains(un.Id));
        await updateQuery.ExecuteUpdateAsync(s => s.SetProperty(un => un.ReadAt, DateTimeOffset.UtcNow));
    }

    /// <inheritdoc/>
    public async Task MarkAsUnreadAsync(Guid userId, IEnumerable<Guid>? notificationIds = null)
    {
        if (notificationIds is not null && notificationIds.Any())
        {
            var idList = notificationIds.ToList();
            bool hasUnauthorizedNotifications = await _context.Notifications
                .AnyAsync(n => idList.Contains(n.Id) && n.NotifiableId != userId);

            if (hasUnauthorizedNotifications)
                throw new ServiceException(StatusCodes.Status403Forbidden, "One or more notification IDs do not belong to the current user");
        }

        var updateQuery = _context.Notifications.Where(un => un.NotifiableId == userId);
        if (notificationIds is not null && notificationIds.Any())
            updateQuery = updateQuery.Where(un => notificationIds.Contains(un.Id));
        await updateQuery.ExecuteUpdateAsync(s => s.SetProperty(un => un.ReadAt, (DateTimeOffset?)null));
    }
}
