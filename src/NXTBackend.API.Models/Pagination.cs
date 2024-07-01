using System.Text.Json.Serialization;

namespace NXTBackend.API.Models;

/// <summary>
/// Query parameters for pagination.
/// </summary>
[JsonSerializable(typeof(PaginationParams))]
public class PaginationParams
{
    private const int c_MaxPageSize = 50;

    private int _pageSize = 30;

    [JsonPropertyName("page")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("size")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(c_MaxPageSize, value);
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
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        int count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
        //return await Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize));
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public IReadOnlyCollection<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }
}