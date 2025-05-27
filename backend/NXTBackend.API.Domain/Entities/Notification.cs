// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXTBackend.API.Domain.Entities;

[Table("tbl_notifications")]
public class Notification : BaseEntity
{
	public Notification()
	{
		Data = "{}";
		ReadAt = null;
		Type = nameof(Notification);
		State = NotificationState.Unprocessed;
		Descriptor = NotificationKind.Default;
    }

    /// <summary>
    /// The notification class.
    /// </summary>
    [Column("type")]
    public string Type { get; set; }
	
	/// <summary>
    /// The notification 
    /// </summary>
    [Column("descriptor")]
    public NotificationKind Descriptor { get; set; }

    /// <summary>
	/// Notifications get dispatched at a set interval, this marks that it has been processed.
	/// </summary>
	[Column("state")]
    public NotificationState State { get; set; }

    /// <summary>
    /// When the notification was read
    /// </summary>
    [Column("read_at")]
    public DateTimeOffset? ReadAt { get; set; }

    /// <summary>
    /// The entity to be notified.
    /// </summary>
    [Column("notifiable_id")]
    public Guid NotifiableId { get; set; }

    /// <summary>
    /// This notification targets a specific resource as context.
    /// </summary>
    [Column("resource_id")]
    public Guid ResourceId { get; set; }

    /// <summary>
    /// The notification data.
    /// </summary>
    [Column("data", TypeName = "jsonb")]
    public string Data { get; set; }
}
