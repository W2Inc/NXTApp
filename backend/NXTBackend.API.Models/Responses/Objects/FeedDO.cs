// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class FeedDO(Feed feed) : BaseObjectDO<Feed>(feed)
{
	/// <summary>
	/// The kind of feed this represents.
	/// </summary>
	[Required]
	public FeedKind FeedKind { get; set; } = feed.Kind;

	/// <summary>
	/// Optional reference to a resource this feed is about (e.g. a project, goal, etc.).
	/// </summary>
	[Required]
	public Guid? ResourceId { get; set; } = feed.ResourceId;

	/// <summary>
	/// The ID of the entity that this feed is notifying about.
	/// </summary>
	[Required]
	public Guid? NotifiableId { get; set; } = feed.NotifiableId;

	/// <summary>
	/// Implicit operator to convert a Feed entity to a FeedDO.
	/// </summary>
	/// <param name="entity">The Feed entity to convert.</param>
	public static implicit operator FeedDO?(Feed? entity) => entity is null ? null : new(entity);
}
