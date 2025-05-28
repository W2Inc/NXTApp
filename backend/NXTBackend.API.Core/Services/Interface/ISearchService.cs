// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Responses.Objects.SearchResponses;
using NXTBackend.API.Core.Utils.Query;

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
    Task<IEnumerable<SearchResultDO>> SearchAsync(string query, PaginationParams pagination, SearchKind? category = null);
}
