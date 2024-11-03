// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Event;

[Table("tbl_notification_action")]
public class NotificationAction : BaseEntity
{
    public NotificationAction()
    {
        UserId = Guid.Empty;
        NotificationId = Guid.Empty;
        IsDismissed = false;
    }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("notification_id")]
    public Guid NotificationId { get; set; }

    [Required]
    [Column("is_dismissed")]
    public bool IsDismissed { get; set; }
}
