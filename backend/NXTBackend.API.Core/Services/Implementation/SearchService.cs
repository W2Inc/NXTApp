using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class SearchService(DatabaseContext ctx) : ISearchService
{
    public async Task<IEnumerable<T>> SearchAsync<T>(SearchRequestDTO data, PaginationParams pagination, Func<DbSet<T>, IQueryable<T>> query) where T : BaseEntity
    {
        var source = query(ctx.Set<T>());
        var items = await source.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
        return items;
    }
}
