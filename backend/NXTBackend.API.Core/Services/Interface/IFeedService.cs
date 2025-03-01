using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Responses.Objects.FeedResponses;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the feed entity.
/// </summary>
public interface IFeedService : IDomainService<Feed>
{
    /// <summary>
    /// Constructs the feed directly into DOs. Mainly because it also additionally needs
    /// to fetch the underlying resourceId depending on the type of Feed it is.
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="sorting"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public Task<IEnumerable<FeedDO>> ConstructFeed(PaginationParams pagination, SortingParams sorting, FilterDictionary? filters = null);
}
