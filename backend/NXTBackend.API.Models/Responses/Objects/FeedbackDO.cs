// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Models.Responses.Objects;

public class FeedbackDO : BaseObjectDO<Feedback>
{
    public FeedbackDO(Feedback feedback) : base(feedback)
    {
        ReviewId = feedback.ReviewId;
        Review = feedback.Review;
    }

    public Guid ReviewId { get; set; }

    public virtual ReviewDO? Review { get; set; }

    public static implicit operator FeedbackDO?(Feedback? entity) => entity is null ? null : new(entity);
}
