using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class FeatureService : IFeatureService
{
    private readonly DatabaseContext _databaseContext;

    public FeatureService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<Feature> CreateAsync(Feature entity)
    {
        var feature = await _databaseContext.Features.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return feature.Entity;
    }

    /// <inheritdoc/>
    public async Task<Feature> DeleteAsync(Feature entity)
    {
        var feature = _databaseContext.Features.Remove(entity);
        await _databaseContext.SaveChangesAsync();
        return feature.Entity;
    }
    
    /// <inheritdoc/>
    public async Task<Feature?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.Features.FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public Task<Feature?> FindByNameAsync(string name)
    {
        return _databaseContext.Features.FirstOrDefaultAsync(u => u.Name == name);
    }

    /// <inheritdoc/>
    public Task<PaginatedList<Feature>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Features.Select(Feature => Feature);
        return PaginatedList<Feature>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public Task<PaginatedList<Feature>> GetAllAsync(PaginationParams pagination, FilterParams filters, OrderByParams order)
    {
        var query = _databaseContext.Features.Select(Feature => Feature);
        //query = OrderQuery<Feature>.ApplyOrder(query, order);
        query = FilterQuery<Feature>.ApplyFilters(query, filters);
        return PaginatedList<Feature>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc/>
    public async Task<Feature> UpdateAsync(Feature entity)
    {
        var feature = _databaseContext.Features.Remove(entity);
        await _databaseContext.SaveChangesAsync();
        return feature.Entity;
    }
}
