// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Models.Responses.Objects;

public class CommentDO : BaseObjectDO<Comment>
{
    public CommentDO(Comment comment) : base(comment)
    {
        User = comment.User;
        Markdown = comment.Markdown;
        ParentId = comment.ParentId;
    }

    [Required]
    public string Markdown { get; set; }

    [Required]
    public virtual MinimalUserDTO User { get; set; }

    [Required]
    public Guid ParentId { get; set; }

    public static implicit operator CommentDO?(Comment? entity) => entity is null ? null : new(entity);
}
