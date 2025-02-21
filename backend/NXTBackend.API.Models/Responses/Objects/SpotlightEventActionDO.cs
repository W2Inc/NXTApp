// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Spotlight;

namespace NXTBackend.API.Models.Responses.Objects;

public class SpotlightEventActionDO : BaseObjectDO<SpotlightEventAction>
{
    public SpotlightEventActionDO(SpotlightEventAction notificationAction) : base(notificationAction)
    {
        UserId = notificationAction.UserId;
        NotificationId = notificationAction.SpotlightId;
        IsDismissed = notificationAction.IsDismissed;
    }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid NotificationId { get; set; }

    [Required]
    public bool IsDismissed { get; set; }
}
