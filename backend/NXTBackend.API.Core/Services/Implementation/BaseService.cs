using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Base service for all services.
///
/// This allows you do to basic CRUD operations on a specific model.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseService<T>(DatabaseContext context) : IDomainService<T> where T : BaseEntity
{
    protected readonly DatabaseContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    /// <inheritdoc />
    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public virtual async Task<T> CreateAsync(T entity)
    {
        var createdEntity = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    /// <inheritdoc />
    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<T> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Depending on the model. It may or may not support a selection of
    /// query filters such as on Slug, ID, Name, ...
    /// <para>
    /// In order to avoid reflection. Every service *may* define / override this
    /// function to apply common QueryFilters in a way that makes sense.
    /// </para>
    /// If the model doesn't have a applicable query (e.g, Users don't have a slug column)
    /// then it is simply not applied / applicable.
    /// </summary>
    /// <param name="query">An exisiting query</param>
    /// <returns>The query with any where clauses it may have added.</returns>
    public virtual IQueryable<T> ApplyFilters(IQueryable<T> query, QueryFilters? filter)
    {
        // NOTE(W2): At the base level we support filtering on id's as it's shared amongst
        // all entities.
        if (filter?.Id is not null)
            query = query.Where(e => e.Id == filter.Id);
        return query;
    }

    /// <inheritdoc />
    public virtual async Task<PaginatedList<T>> GetAllAsync(PaginationParams pagination, SortingParams sorting, QueryFilters? filter = null)
    {
        var query = ApplyFilters(_dbSet.AsQueryable(), filter);
        query = await SortedList<T>.ApplyAsync(query, sorting);
        return await PaginatedList<T>.CreateAsync(query, pagination.Page, pagination.Size);
    }
}
