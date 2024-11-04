// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Notification;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class NotificationDO(Notification notification) : BaseDO<Notification>(notification)
{
    public string Title { get; set; } = notification.Title;

    public string Description { get; set; } = notification.Description;

    public string ActionText { get; set; } = notification.ActionText;

    public string Href { get; set; } = notification.Href;

    public string BackgroundUrl { get; set; } = notification.BackgroundUrl;

    public static implicit operator NotificationDO?(Notification? entity) => entity is null ? null : new(entity);
}
