// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Notification;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class NotificationDO : BaseDO<Notification>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string ActionText { get; set; }

    public string Href { get; set; }

    public string BackgroundUrl { get; set; }

    public NotificationDO(Notification notification) : base(notification)
    {
        Title = notification.Title;
        Description = notification.Description;
        ActionText = notification.ActionText;
        Href = notification.Href;
        BackgroundUrl = notification.BackgroundUrl;
    }

    public static implicit operator NotificationDO?(Notification? entity) => entity is null ? null : new(entity);
}
