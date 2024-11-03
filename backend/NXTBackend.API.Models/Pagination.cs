using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace NXTBackend.API.Models;

/// <summary>
/// Query parameters for pagination.
/// </summary>
public class PaginationParams
{
    private const int c_MaxPageSize = 50;

    private int _pageSize = 30;
    private int _pageNumber = 1;

    /// <summary>
    /// The page number.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Page
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(1, value);
    }

    /// <summary>
    /// How many items per page.
    /// </summary>
    [Range(1, c_MaxPageSize)]
    public int Size
    {
        get => _pageSize;
        set => _pageSize = Math.Min(c_MaxPageSize, Math.Max(1, value));
    }
}

/// <summary>
/// Paginated list of items.
/// </summary>
/// <typeparam name="T">The object type</typeparam>
public class PaginatedList<T>
{
    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        Page = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    /// <summary>
    /// Create a paginated list from a queryable source.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    /// <summary>
    /// Append pagination headers to the response headers.
    /// </summary>
    /// <param name="headers"></param>
    public void AppendHeaders(IDictionary<string, StringValues> headers)
    {
        headers.Add("X-Page", Page.ToString());
        headers.Add("X-Pages", TotalPages.ToString());

    }

    public IReadOnlyCollection<T> Items { get; }

    public int Page { get; }

    public int TotalPages { get; }
}