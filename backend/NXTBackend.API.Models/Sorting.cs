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
/// The different kinds of cursi that exist.
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
/// Query parameters for pagination.
/// </summary>
public class SortingParams
{
    /// <summary>
    /// On which field to order the resource.
    ///
    /// For instance if on a string, it will try to sort it alphabetically.
    /// If a number, it will sort it numerically.
    ///
    /// And so on.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// In which order to sort the resource.
    ///
    /// From lowest to highest or from highest to lowest.
    /// </summary>
    public Order Order { get; set; } = Order.Ascending;
}

/// <summary>
/// Paginated list of items.
/// </summary>
/// <typeparam name="T">The object type</typeparam>
public class SortedList<T> where T : BaseEntity
{
    public static async Task<IQueryable<T>> ApplyAsync(IQueryable<T> source, SortingParams sorting)
    {
        // Check if there are any items to sort
        if (!await source.AnyAsync())
            return source;

        // If no orderByQueryString is provided, return source without ordering
        if (string.IsNullOrWhiteSpace(sorting.OrderBy))
            return source;

        // Parse and build the sorting expression
        string[] orderParams = sorting.OrderBy.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        IOrderedQueryable<T>? orderedQuery = null;
        foreach (string param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;

            string[] paramParts = param.Trim().Split(" ");
            string propertyName = paramParts[0];
            bool descending = paramParts.Length > 1 && paramParts[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase);

            var property = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (property == null) continue;

            // Build the expression dynamically
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // Apply OrderBy or ThenBy based on whether it is the first sorting parameter
            orderedQuery = orderedQuery == null
                ? (IOrderedQueryable<T>)(descending
                    ? Queryable.OrderByDescending(source, (dynamic)orderByExpression)
                    : Queryable.OrderBy(source, (dynamic)orderByExpression))
                : (IOrderedQueryable<T>)(descending
                    ? Queryable.ThenByDescending(orderedQuery, (dynamic)orderByExpression)
                    : Queryable.ThenBy(orderedQuery, (dynamic)orderByExpression));
        }

        return orderedQuery ?? source; // Return sorted query if applicable, otherwise original source
    }
}
