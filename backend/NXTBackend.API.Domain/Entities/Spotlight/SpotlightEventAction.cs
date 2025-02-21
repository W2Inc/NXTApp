// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXTBackend.API.Domain.Entities.Spotlight;

[Table("tbl_spotlight_event_action")]
public class SpotlightEventAction : BaseEntity
{
    public SpotlightEventAction()
    {
        UserId = Guid.Empty;
        SpotlightId = Guid.Empty;
        IsDismissed = false;
    }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("spotlight_id")]
    public Guid SpotlightId { get; set; }

    [Column("is_dismissed")]
    public bool IsDismissed { get; set; }
}
