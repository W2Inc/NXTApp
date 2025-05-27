// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Models;

/// <summary>
/// The different kinds of order that exist.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Order
{
    /// <summary>
    /// Order the requested resource in ascending order.
    /// </summary>
    [JsonPropertyName(nameof(Ascending))]
    Ascending,

    /// <summary>
    /// Order the requested resource in descending order.
    /// </summary>
    [JsonPropertyName(nameof(Descending))]
    Descending,
}

/// <summary>
/// Query parameters for sorting.
/// </summary>
public class SortingParams
{
    /// <summary>
    /// On which field to order the resource.
    ///
    /// For instance if on a string, it will try to sort it alphabetically.
    /// If a number, it will sort it numerically.
    ///
    /// Supports multiple fields separated by commas: "name,createdAt"
    /// Individual fields can specify direction: "name desc,createdAt asc"
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Default order direction when not specified per field.
    /// Individual fields in OrderBy can override this with "desc" or "asc".
    /// </summary>
    public Order Order { get; set; } = Order.Ascending;
}

/// <summary>
/// Utility class for applying sorting to queryables.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public static class SortedList<T> where T : BaseEntity
{
    public static IQueryable<T> Apply(IQueryable<T> source, SortingParams sorting)
    {
        if (string.IsNullOrWhiteSpace(sorting.OrderBy))
        {
            var defaultKeySelector = T.GetKeySelectorFor("id");
            if (defaultKeySelector != null)
            {
                return sorting.Order == Order.Descending 
                    ? source.OrderByDescending(defaultKeySelector)
                    : source.OrderBy(defaultKeySelector);
            }
            return source;
        }
        
        // If no orderBy is provided, return source ordered by Id (default)

        //
        // // ... rest of implementation stays the same
        // string[] orderParams = sorting.OrderBy.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
        // IOrderedQueryable<T>? orderedQuery = null;
        //
        // foreach (string param in orderParams)
        // {
        //     if (string.IsNullOrWhiteSpace(param)) continue;
        //
        //     string[] paramParts = param.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //     string sortKey = paramParts[0];
        //     
        //     bool descending = paramParts.Length > 1 
        //         ? paramParts[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase)
        //         : sorting.Order == Order.Descending;
        //
        //     var keySelector = T.GetKeySelectorFor(sortKey);
        //     if (keySelector == null) continue;
        //
        //     if (orderedQuery == null)
        //     {
        //         orderedQuery = descending 
        //             ? source.OrderByDescending(keySelector)
        //             : source.OrderBy(keySelector);
        //     }
        //     else
        //     {
        //         orderedQuery = descending
        //             ? orderedQuery.ThenByDescending(keySelector)
        //             : orderedQuery.ThenBy(keySelector);
        //     }
        // }
        //
        // return orderedQuery ?? source;
    }

    public static Task<IQueryable<T>> ApplyAsync(IQueryable<T> source, SortingParams sorting)
    {
        return Task.FromResult(Apply(source, sorting));
    }
}