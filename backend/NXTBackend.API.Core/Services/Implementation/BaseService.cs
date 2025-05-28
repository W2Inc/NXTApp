using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Utils.Query;
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
	protected readonly Dictionary<string, Func<IQueryable<T>, object?, IQueryable<T>>> _filterHandlers;

	public BaseService(DatabaseContext context)
	{
		_context = context;
		_dbSet = context.Set<T>();
		_filterHandlers = new()
		{
			["id"] = (query, value) => value is Guid id ? query.Where(e => e.Id == id) : query,
			["created_at"] = (query, value) => value is DateTimeOffset date ? query.Where(e => e.CreatedAt == date) : query,
			["updated_at"] = (query, value) => value is DateTimeOffset date ? query.Where(e => e.UpdatedAt == date) : query
		};
	}

	public IQueryable<T> Query(bool tracking = true)
	{
		return tracking ? _dbSet : _dbSet.AsNoTracking();
	}

    /// <summary>
	/// Registers a new filter handler that can perform any query transformation
	/// </summary>
	/// <typeparam name="TValue">The type of the filter value</typeparam>
	/// <param name="key">The filter key</param>
	/// <param name="queryTransform">The function that transforms the query</param>
	protected void DefineFilter<TValue>(string key, Func<IQueryable<T>, TValue, IQueryable<T>> queryTransform)
	{
		_filterHandlers[key.ToLowerInvariant()] = (query, value) =>
		{
			if (value is TValue typedValue)
				return queryTransform(query, typedValue);
			return query;
		};
	}
	protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query, FilterDictionary? filters)
	{
		if (filters is null || !filters.Any())
			return query;

		foreach (var filter in filters)
			if (_filterHandlers.TryGetValue(filter.Key.ToLowerInvariant(), out var handler))
				query = handler(query, filter.Value);
		return query;
	}

	public virtual async Task<T?> FindByIdAsync(Guid id)
	{
		return await Query(false).FirstOrDefaultAsync(x => x.Id == id);
	}

	public virtual async Task<bool> AreValid(IEnumerable<Guid> ids)
	{
		if (!ids.Any())
			return true;

		var idSet = new HashSet<Guid>(ids);
		var existingCount = await Query(false)
			.Where(p => idSet.Contains(p.Id))
			.CountAsync();

		return existingCount == idSet.Count;
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

	public virtual async Task<PaginatedList<T>> GetAllAsync(
		PaginationParams pagination,
		SortingParams sorting,
		FilterDictionary? filters = null)
	{
        Console.WriteLine("\n\n\n\n\nja\n\n\n\n\n");
		var query = ApplyFilters(Query(), filters);
        query = SortedList<T>.Apply(query, sorting);
		return await PaginatedList<T>.CreateAsync(query, pagination.Page, pagination.Size);
	}
}
