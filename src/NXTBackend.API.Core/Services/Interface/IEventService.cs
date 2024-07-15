using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IEventService : IDomainService<Event>
{
    /// <summary>
    /// Find the feature by its name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns> The feature or null</returns>
    public Task<Event?> FindByNameAsync(string name);

    public Task<PaginatedList<Event>> GetAllAsync(PaginationParams pagination, FilterParams filters, OrderByParams order);
}