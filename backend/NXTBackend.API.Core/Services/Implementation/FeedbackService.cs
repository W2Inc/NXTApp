// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// FeedbackService
/// </summary>
public class FeedbackService : BaseService<Feedback>, IFeedbackService
{
    public FeedbackService(DatabaseContext context) : base(context)
    {

    }
}
