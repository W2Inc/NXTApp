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
public abstract class BaseService<T> : IDomainService<T> where T : BaseEntity
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseService(DatabaseContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        var createdEntity = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<PaginatedList<T>> GetAllAsync(PaginationParams pagination, SortingParams sorting)
    {
        var query = await SortedList<T>.ApplyAsync(_dbSet.AsQueryable(), sorting);
        return await PaginatedList<T>.CreateAsync(query, pagination.Page, pagination.Size);
    }
}
