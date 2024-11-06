// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Models.Responses.Objects;

public class CommentDO : BaseObjectDO<Comment>
{
    public CommentDO(Comment comment) : base(comment)
    {
        Markdown = comment.Markdown;
        UserId = comment.UserId;
        User = comment.User;
        FeedbackId = comment.FeedbackId;
        Feedback = comment.Feedback;
    }

    public string Markdown { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public Guid FeedbackId { get; set; }

    public virtual Feedback Feedback { get; set; }

    public static implicit operator CommentDO?(Comment? entity) => entity is null ? null : new(entity);
}
