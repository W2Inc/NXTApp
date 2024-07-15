// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.XPath;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;

// ============================================================================

namespace NXTBackend.API.Models;

/// <summary>
/// Query parameters for pagination.
/// </summary>
public class FilterParams
{
    /// <summary>
    /// Filter by id.
    /// </summary>
    [JsonPropertyName("filter[id]")]
    public Guid Id { get; set; }

    /// <summary>
    /// Filter by created at.
    /// </summary>
    [JsonPropertyName("filter[created_at]")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Filter by updated at.
    /// </summary>
    [JsonPropertyName("filter[updated_at]")]
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Paginated list of items.
/// </summary>
/// <typeparam name="T">The object type</typeparam>
public class FilterQuery<T> where T : BaseEntity
{
    public static IQueryable<T> ApplyFilters(IQueryable<T> source, FilterParams filters)
    {
        if (filters.Id != Guid.Empty)
            source = source.Where(e => EF.Property<Guid>(e, "Id") == filters.Id);

        if (filters.CreatedAt != default)
            source = source.Where(e => EF.Property<DateTimeOffset>(e, "CreatedAt") == filters.CreatedAt);

        if (filters.UpdatedAt != default)
            source = source.Where(e => EF.Property<DateTimeOffset>(e, "UpdatedAt") == filters.UpdatedAt);

        return source;
    }
}

// ============================================================================

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderBy
{
    [JsonPropertyName(nameof(Asc))]
    Asc,

    [JsonPropertyName(nameof(Desc))]
    Desc
}

/// <summary>
/// Query parameters for pagination.
/// </summary>
public class OrderByParams
{
    [JsonPropertyName("sort[field]")]
    public string? Field { get; set; }

    [JsonPropertyName("sort[order]")]
    public OrderBy Order { get; set; }
}