// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using System.Text.Json.Serialization;


namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

[JsonDerivedType(typeof(NewUserFeedDO), typeDiscriminator: nameof(NewUserFeedDO))]
[JsonDerivedType(typeof(CompletedProjectFeedDO), typeDiscriminator: nameof(CompletedProjectFeedDO))]
// Add other derived types as needed
public abstract class FeedDO : BaseObjectDO<Feed>
{
    protected FeedDO(Feed feed) : base(feed)
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

    // Factory method to create the appropriate derived type
    public static FeedDO Create(Feed feed) => feed.Kind switch
    {
        FeedKind.NewUser => new NewUserFeedDO(feed),
        FeedKind.CompletedProject => new CompletedProjectFeedDO(feed),
        // Add other cases as needed
        _ => throw new ArgumentOutOfRangeException(nameof(feed.Kind), feed.Kind, "Unknown feed kind")
    };
}
