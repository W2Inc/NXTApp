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
public sealed class EventService : IEventService
{
    private readonly DatabaseContext _databaseContext;

    public EventService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Event> CreateAsync(Event entity)
    {
        var eventData = await _databaseContext.Events.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return eventData.Entity;
    }

    public async Task<Event> DeleteAsync(Event entity)
    {
        // Delete all user actions on that event
        var eventData = _databaseContext.Events.Remove(entity).Entity;
        await _databaseContext.EventActions
            .Where(ua => ua.EventId == entity.Id)
            .ExecuteDeleteAsync();

        await _databaseContext.SaveChangesAsync();
        return eventData;
    }

    public async Task<Event?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.Events.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Event?> FindByNameAsync(string name)
    {
        return await _databaseContext.Events.FirstOrDefaultAsync(u => u.Title == name);
    }

    public Task<PaginatedList<Event>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Events.Select(Event => Event);
        return PaginatedList<Event>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<Event> UpdateAsync(Event entity)
    {
        var eventData = _databaseContext.Events.Update(entity).Entity;
        await _databaseContext.SaveChangesAsync();
        return eventData;
    }
}
