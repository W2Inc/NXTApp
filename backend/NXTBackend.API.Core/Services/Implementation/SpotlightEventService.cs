using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class SpotlightEventService(DatabaseContext ctx) : BaseService<SpotlightEvent>(ctx), ISpotlightEventService
{
    /// <inheritdoc />
    public async override Task<SpotlightEvent> DeleteAsync(SpotlightEvent entity)
    {
        // Delete all user actions on that event
        var eventData = _dbSet.Remove(entity).Entity;
        await _context.SpotlightEventsActions
            .Where(ua => ua.SpotlightId == entity.Id)
            .ExecuteDeleteAsync();

        await _context.SaveChangesAsync();
        return eventData;
    }
}
