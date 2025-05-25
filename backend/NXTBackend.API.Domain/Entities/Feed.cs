// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities;

/// <summary>
/// A global event feed for all users.
///
/// Feeds notify all users on the application on something that has happened.
/// e.g: User X finished this project, goal, cursus whatever it may be.
///
/// Feeds can be customized via the UserFeedConfiguration.
/// In which they set up a mask of what feed types they want to have.
/// </summary>
[Table("tbl_feed")]
public class Feed : BaseEntity
{
	public Feed()
	{
		ResourceId = null;
		NotifiableId = null;
		Kind = FeedKind.Default;
    }

    [Column("notifiable_id")]
    public Guid? NotifiableId { get; set; }

    /// <summary>
	/// The kind of feed this represents
	/// </summary>
	[Column("type")]
    public FeedKind Kind { get; set; }

    /// <summary>
    /// Optional reference to a resource this feed is about (e.g. a project, goal, etc.).
    /// </summary>
    [Column("resource_id")]
    public Guid? ResourceId { get; set; }
}
