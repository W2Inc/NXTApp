using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests.Event;

public class EventPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Title { get; set; }

    /// <summary>
    /// A small description of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Description { get; set; }

    /// <summary>
    /// Upon the button to learn more, what should it say?
    /// </summary>
    [Required, StringLength(64, MinimumLength = 4)]
    public string HrefText { get; set; }

    /// <summary>
    /// Where should the button redirect the user?
    /// </summary>
    [Required, Url]
    public string Href { get; set; }

    /// <summary>
    /// The background image of the event
    /// </summary>
    [Required, Url]
    public string BackgroundUrl { get; set; }
}