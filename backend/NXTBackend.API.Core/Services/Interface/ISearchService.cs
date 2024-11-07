
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace NXTBackend.API.Core.Services.Interface;

public interface ISearchService
{
    /// <summary>
    /// Generic search function that takes in a function to query the given entity type.
    /// </summary>
    /// <typeparam name="T">An model entity.</typeparam>
    /// <param name="data">The DTO</param>
    /// <param name="pagination">The pagination parameters</param>
    /// <param name="sorting">The sorting parameters</param>
    /// <param name="query">The query used to search for on the model.</param>
    /// <returns>A enumarable value of possible results.</returns>
    Task<IEnumerable<T>> SearchAsync<T>(SearchRequestDTO data, PaginationParams pagination, Func<DbSet<T>, IQueryable<T>> query) where T : BaseEntity;
}
