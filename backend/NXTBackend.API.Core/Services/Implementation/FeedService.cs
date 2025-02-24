using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <inheritdoc />
public sealed class FeedService : BaseService<Feed>, IFeedService
{
    public FeedService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<FeedKind>("type", (q, mask) => q.Where(n => (n.Kind & mask) == n.Kind));
    }
}
