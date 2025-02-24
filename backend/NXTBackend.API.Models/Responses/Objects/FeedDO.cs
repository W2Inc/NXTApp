// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Responses.Objects;

public class FeedDO : BaseObjectDO<Feed>
{
    public FeedDO(Feed feed) : base(feed)
    {
        Kind = feed.Kind;
        Actor = feed.Actor;
        ResourceId = feed.ResourceId;
    }

    [Required]
    public FeedKind Kind { get; set; }

    [Required]
    public MinimalUserDTO? Actor { get; set; }

    [Required]
    public Guid? ResourceId { get; set; }

    public static implicit operator FeedDO?(Feed? entity) => entity is null ? null : new(entity);
}
