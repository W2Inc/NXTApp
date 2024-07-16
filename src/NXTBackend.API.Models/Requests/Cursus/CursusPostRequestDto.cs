// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Cursus;

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class CursusPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A small description of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 128)]
    public string Markdown { get; set; } = string.Empty;

    /// <summary>
    /// Is the cursus visible to the public?
    /// </summary>
    [Required]
    public bool Public { get; set; } = false;

    /// <summary>
    /// Can a user actively subscribe to the cursus?
    /// </summary>
    [Required]
    public bool Enabled { get; set; } = false;
}