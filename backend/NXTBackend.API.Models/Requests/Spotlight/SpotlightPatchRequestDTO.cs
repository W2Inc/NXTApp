using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests.Spotlight;

/*
    pub title: String,
    pub description: String,
    pub action_text: String,
    pub href: String,
    pub background_url: Option<String>,
*/

public class SpotlightPatchRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Title { get; set; }

    /// <summary>
    /// A small description of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Description { get; set; }

    /// <summary>
    /// Upon the button to learn more, what should it say?
    /// </summary>
    [StringLength(64, MinimumLength = 4)]
    public string? HrefText { get; set; }

    /// <summary>
    /// Where should the button redirect the user?
    /// </summary>
    [Url]
    public string? Href { get; set; }

    /// <summary>
    /// The background image of the event
    /// </summary>
    [Url]
    public string? BackgroundUrl { get; set; }
}
