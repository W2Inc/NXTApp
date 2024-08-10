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
public sealed class EventActionService : IEventActionService
{
    private readonly DatabaseContext _databaseContext;

    public EventActionService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<EventAction> CreateAsync(EventAction entity)
    {
        var eventData = await _databaseContext.EventActions.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return eventData.Entity;
    }

    public async Task<EventAction> DeleteAsync(EventAction entity)
    {
        // Delete all user actions on that event
        var eventData = _databaseContext.EventActions.Remove(entity).Entity;
        await _databaseContext.EventActions
            .Where(ua => ua.EventId == entity.Id)
            .ExecuteDeleteAsync();

        await _databaseContext.SaveChangesAsync();
        return eventData;
    }

    public async Task<EventAction?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.EventActions.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<PaginatedList<EventAction>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.EventActions.Select(Event => Event);
        return PaginatedList<EventAction>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<EventAction> UpdateAsync(EventAction entity)
    {
        var eventData = _databaseContext.EventActions.Update(entity).Entity;
        await _databaseContext.SaveChangesAsync();
        return eventData;
    }
}
