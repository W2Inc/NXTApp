// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Models.Requests.Cursus;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// Patch request for updating a cursus
/// </summary>
public class CursusPatchRequestDto : BaseRequestDto
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; } = string.Empty;

    /// <summary>
    /// A small description of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; } = string.Empty;

    /// <summary>
    /// Is the cursus visible to the public?
    /// </summary>
    public bool? Public { get; set; } = false;

    /// <summary>
    /// Can a user actively subscribe to the cursus?
    /// </summary>
    public bool? Enabled { get; set; } = false;

    /// <summary>
    /// The path of the cursus
    /// </summary>
    public CursusPath? Path { get; set; }
}