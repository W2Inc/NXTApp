using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IFeatureService : IDomainService<Feature>
{
    /// <summary>
    /// Find the feature by its name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns> The feature or null</returns>
    public Task<Feature?> FindByNameAsync(string name);

    public Task<PaginatedList<Feature>> GetAllAsync(PaginationParams pagination, FilterParams filers, OrderByParams order);
}