// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Infrastructure.Database;

// ============================================================================

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CommentService : BaseService<Comment>, ICommentService
{
    public CommentService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("parent_id", (q, id) => q.Where((c) => c.ParentId == id));
    }
}
