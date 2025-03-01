using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Responses.Objects.FeedResponses;

namespace NXTBackend.API.Core.Services.Implementation;

/// <inheritdoc />
public sealed class FeedService : BaseService<Feed>, IFeedService
{
    public FeedService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<FeedKind>("type", (q, mask) => q.Where(n => (n.Kind & mask) == n.Kind));
    }


    public async Task<IEnumerable<FeedDO>> ConstructFeed(PaginationParams pagination, SortingParams sorting, FilterDictionary? filters = null)
    {
        // NOTE: To avoid concurrency issues due to DBContext
        var page = await GetAllAsync(pagination, sorting, filters);

        var result = new List<FeedDO>();
        foreach (var feed in page.Items)
            result.Add(FeedDO.Create(feed, await FindResource(feed)));
        return result;
    }

    /// <summary>
    /// Loads the appropriate resource for a feed item based on its kind
    /// </summary>
    private async Task<BaseEntity?> FindResource(Feed feed)
    {
        return feed.Kind switch
        {
            FeedKind.NewUser => await _context.Users.FindAsync(feed.ResourceId),
            FeedKind.CompletedProject => await _context.Projects.FindAsync(feed.ResourceId),
            FeedKind.CompletedGoal => await _context.LearningGoals.FindAsync(feed.ResourceId),
            FeedKind.CompletedCursus => await _context.Cursi.FindAsync(feed.ResourceId),
            FeedKind.ReceivedReview => await _context.Reviews.FindAsync(feed.ResourceId),
            FeedKind.ProjectDeprecated => await _context.Projects.FindAsync(feed.ResourceId),
            FeedKind.GoalDeprecated => await _context.LearningGoals.FindAsync(feed.ResourceId),
            FeedKind.CursusDeprecated => await _context.Cursi.FindAsync(feed.ResourceId),
            _ => throw new ServiceException(StatusCodes.Status501NotImplemented, $"{nameof(feed.Kind)} is not implemented.")
        };
    }
}
