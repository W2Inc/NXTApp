// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Domain.Common;

// ============================================================================

namespace NXTBackend.API.Core.Utils.Query;

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

// ============================================================================

/// <summary>
/// Represents parameters used for sorting query results.
/// </summary>
/// <remarks>
/// This class provides properties to specify both the field to sort by and the sorting direction.
/// </remarks>
public class SortingParams
{
    /// <summary>
    /// Gets or sets the name of the property or field to order results by.
    /// </summary>
    /// <value>The name of the property to use for sorting.</value>
    [FromQuery( Name = "sort_by")]
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the sort direction (ascending or descending).
    /// </summary>
    /// <value>The sort direction. Defaults to <see cref="Order.Ascending"/>.</value>
    [FromQuery( Name = "sort")]
    public Order Order { get; set; } = Order.Ascending;
}

// ============================================================================

/// <summary>
/// Utility class for applying sorting to queryables.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public static class SortedList<T> where T : BaseEntity
{
    /// <summary>
    /// Applies sorting to a queryable collection based on the provided sorting parameters.
    /// </summary>
    /// <param name="source">The source queryable to sort.</param>
    /// <param name="sorting">The sorting parameters to apply.</param>
    /// <returns>A sorted queryable.</returns>
    public static IQueryable<T> Apply(IQueryable<T> source, SortingParams sorting)
    {
        try
        {
            // Use default sorting if no valid sort parameters are provided
            if (string.IsNullOrWhiteSpace(sorting.OrderBy))
                return ApplyDefaultSorting(source, sorting.Order);

            // Process each sort parameter
            var orderParams = sorting.OrderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (orderParams.Length == 0)
                return ApplyDefaultSorting(source, sorting.Order);

            var defaultDirection = sorting.Order == Order.Descending ? "desc" : "asc";
            var sortExpressions = new List<string>(orderParams.Length);

            foreach (var param in orderParams)
            {
                var parts = param.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var field = parts[0];
                var direction = parts.Length > 1 ? parts[1].ToLowerInvariant() : defaultDirection;
                sortExpressions.Add($"{field} {direction}");
            }

            // Apply sorting or default if no valid expressions
            return sortExpressions.Count > 0
                ? source.OrderBy(string.Join(", ", sortExpressions))
                : ApplyDefaultSorting(source, sorting.Order);
        }
        catch (ParseException e)
        {
            throw new ServiceException(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Applies default sorting by Id to a queryable.
    /// </summary>
    private static IQueryable<T> ApplyDefaultSorting(IQueryable<T> source, Order order)
    {
        return source.OrderBy(order == Order.Descending
            ? $"{nameof(BaseEntity.Id)} descending"
            : nameof(BaseEntity.Id));
    }
}
