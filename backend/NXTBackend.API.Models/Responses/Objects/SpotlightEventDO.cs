// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Spotlight;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class SpotlightEventDO : BaseObjectDO<SpotlightEvent>
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ActionText { get; set; }

    [Required]
    public string Href { get; set; }

    [Required]
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
