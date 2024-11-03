// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Models;

// ============================================================================

namespace NXTBackend.API.Core;

/// <summary>
/// Interface for Domain models.
/// </summary>
/// <typeparam name="T">The model type.</typeparam>
public interface IDomainService<T> where T : BaseEntity
{
    /// <summary>
    /// Find the entity by its ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The entity found by that ID or null if not found.</returns>
    public Task<T?> FindByIdAsync(Guid id);

    /// <summary>
    /// Update the entity.
    /// </summary>
    /// <param name="entity">The updated entity.</param>
    /// <returns>The updated entity.</returns>
    public Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Delete the entity.
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <returns>The deleted entity.</returns>
    public Task<T> DeleteAsync(T entity);

    /// <summary>
    /// Create a new entity.
    /// </summary>
    /// <param name="entity">The newly created entity.</param>
    /// <returns>The newly created entity.</returns>
    public Task<T> CreateAsync(T entity);

    /// <summary>
    /// Get all entities.
    /// </summary>
    /// <param name="pagination">Specific pagination parameters (as to avoid large queries)</param>
    /// <returns>A paginated list of entities.</returns>
    public Task<PaginatedList<T>> GetAllAsync(PaginationParams pagination, SortingParams sorting);
}
