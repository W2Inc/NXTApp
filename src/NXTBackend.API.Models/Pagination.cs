using System.Text.Json.Serialization;

namespace NXTBackend.API.Common;

/// <summary>
/// Query parameters for pagination.
/// </summary>
[JsonSerializable(typeof(PaginationParams))]
public class PaginationParams
{
    private int pageSize = 30;
    private const int maxPageSize = 50;

    [JsonPropertyName("page")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("size")]
    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}

/// <summary>
/// Paginated list of items.
/// </summary>
/// <typeparam name="T">The object type</typeparam>
public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public async static Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        int count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
        //return await Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize));
    }
}