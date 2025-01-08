// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Models.Responses.Objects;

public class CommentDO : BaseObjectDO<Comment>
{
    public CommentDO(Comment comment) : base(comment)
    {
        Markdown = comment.Markdown;
        User = comment.User;
        Feedback = comment.Feedback;
    }

    [Required]
    public string Markdown { get; set; }

    [Required]
    public virtual MinimalUserDTO User { get; set; }

    [Required]
    public virtual FeedbackDO Feedback { get; set; }

    public static implicit operator CommentDO?(Comment? entity) => entity is null ? null : new(entity);
}
