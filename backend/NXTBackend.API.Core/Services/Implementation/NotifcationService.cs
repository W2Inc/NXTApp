using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class NotifcationService(DatabaseContext ctx) : BaseService<Notification>(ctx), INotificationService
{
    /// <inheritdoc />
    public async override Task<Notification> DeleteAsync(Notification entity)
    {
        // Delete all user actions on that event
        var eventData = _dbSet.Remove(entity).Entity;
        await _context.NotificationActions
            .Where(ua => ua.NotificationId == entity.Id)
            .ExecuteDeleteAsync();

        await _context.SaveChangesAsync();
        return eventData;
    }

    public async Task<Notification?> FindByTitleAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(n => n.Title == name );
    }
}
