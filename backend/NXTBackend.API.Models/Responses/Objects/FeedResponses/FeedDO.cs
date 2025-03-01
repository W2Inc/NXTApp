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
using NXTBackend.API.Domain.Common;


namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

[JsonDerivedType(typeof(NewUserFeedDO), typeDiscriminator: nameof(FeedKind.NewUser))]
[JsonDerivedType(typeof(CompletedProjectFeedDO), typeDiscriminator: nameof(FeedKind.CompletedProject))]
public abstract class FeedDO(Feed feed) : BaseObjectDO<Feed>(feed)
{
    public static FeedDO Create<T>(Feed feed, T? resource) where T : BaseEntity => feed.Kind switch
    {
        FeedKind.NewUser => new NewUserFeedDO(feed, resource),
        FeedKind.CompletedProject => new CompletedProjectFeedDO(feed, resource),
        // Add other cases as needed
        _ => throw new ArgumentOutOfRangeException(nameof(feed.Kind), feed.Kind, "Unknown feed kind")
    };
}
