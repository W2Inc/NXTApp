// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities.Notification;

[Table("tbl_spotlight_event_action")]
public class SpotlightEventAction : BaseEntity
{
    public SpotlightEventAction()
    {
        UserId = Guid.Empty;
        SpotlightId = Guid.Empty;
        IsDismissed = false;
    }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("spotlight_id")]
    public Guid SpotlightId { get; set; }

    [Required]
    [Column("is_dismissed")]
    public bool IsDismissed { get; set; }
}
