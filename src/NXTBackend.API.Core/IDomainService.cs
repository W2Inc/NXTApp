namespace NXTBackend.API.Core;

using NXTBackend.API.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Requests;

/// <summary>
/// Interface for Domain models.
/// 
/// Provides generic, usable methods for all models such as Create, Update, Delete, FindById and GetAll.
/// </summary>
/// <typeparam name="T">The model.</typeparam>
public interface IDomainService<T>
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
    public T Update(T entity);

    /// <summary>
    /// Delete the entity.
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <returns>The deleted entity.</returns>
    public T Delete(T entity);

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
    public Task<PaginatedList<T>> GetAllAsync(PaginationParams pagination);
}
