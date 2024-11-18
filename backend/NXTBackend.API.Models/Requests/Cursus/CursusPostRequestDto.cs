// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Cursus;

#nullable disable

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class CursusPostRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Name { get; set; }

    /// <summary>
    /// A small description of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Description { get; set; }

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 128)]
    public string Markdown { get; set; }

    /// <summary>
    /// Is the cursus visible to the public?
    /// </summary>
    [Required]
    public bool Public { get; set; }

    /// <summary>
    /// Can a user actively subscribe to the cursus?
    /// </summary>
    [Required]
    public bool Enabled { get; set; }

    /// <summary>
    /// The kind of cursus
    /// </summary>
    [Required]
    public CursusKind Kind { get; set; }
}
