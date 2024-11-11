// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Notification;

namespace NXTBackend.API.Models.Responses.Objects;

public class SpotlightEventActionDO : BaseObjectDO<SpotlightEventAction>
{
    public SpotlightEventActionDO(SpotlightEventAction notificationAction) : base(notificationAction)
    {
        UserId = notificationAction.UserId;
        NotificationId = notificationAction.SpotlightId;
        IsDismissed = notificationAction.IsDismissed;
    }

    public Guid UserId { get; set; }

    public Guid NotificationId { get; set; }

    public bool IsDismissed { get; set; }
}
