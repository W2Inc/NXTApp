// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Notification;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class SpotlightEventDO : BaseObjectDO<SpotlightEvent>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string ActionText { get; set; }

    public string Href { get; set; }

    public string BackgroundUrl { get; set; }

    public SpotlightEventDO(SpotlightEvent notification) : base(notification)
    {
        Title = notification.Title;
        Description = notification.Description;
        ActionText = notification.ActionText;
        Href = notification.Href;
        BackgroundUrl = notification.BackgroundUrl;
    }

    public static implicit operator SpotlightEventDO?(SpotlightEvent? entity) => entity is null ? null : new(entity);
}
