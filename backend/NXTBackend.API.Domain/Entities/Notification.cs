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
		ReadAt = null;
		State = NotificationState.Unprocessed;
	}

	/// <summary>
	/// When the notification was read
	/// </summary>
	[Column("read_at")]
	public DateTimeOffset? ReadAt { get; set; }

	/// <summary>
	/// The notification data.
	/// </summary>
	[Column("type")]
	public string Type { get; set; }
	
	/// <summary>
    /// Notifications get dispatched at a set interval, this marks that it has been processed.
    /// </summary>
    [Column("state")]
	public NotificationState State { get; set; }

	/// <summary>
	/// The notification data.
	/// </summary>
	[Column("data", TypeName = "jsonb")]
	public string Data { get; set; }


    [Column("notifiable_id")]
    public Guid NotifiableId { get; set; }

}
