// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Responses.Objects;

public class ReviewDO : BaseObjectDO<Review>
{
    public ReviewDO(Review review) : base(review)
    {
        Kind = review.Kind;
        State = review.State;
        Validated = review.Validated;
        Reviewer = review.Reviewer;
        // Rubric = review.Rubric;
        // Feedback = review.Feedback;
        UserProjectId = review.UserProject.Id;
    }

    public ReviewKind Kind { get; set; }

    public ReviewState State { get; set; }

    public bool Validated { get; set; }


    public MinimalUserDTO? Reviewer { get; set; }

    // public RubricDO? Rubric { get; set; }

    // public FeedbackDO? Feedback { get; set; }

    public Guid UserProjectId { get; set; }
    // public UserProjectDO? UserProject { get; set; }

    public static implicit operator ReviewDO?(Review? entity) => entity is null ? null : new(entity);
}
