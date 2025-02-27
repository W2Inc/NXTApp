// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Cursus;

/// <summary>
/// Patch request for updating a cursus
/// </summary>
public class CursusPatchRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; } = null;

    /// <summary>
    /// A small description of the event
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Description { get; set; } = null;

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; } = null;

    /// <summary>
    /// Is the cursus visible to the public?
    /// </summary>
    public bool? Public { get; set; } = null;

    /// <summary>
    /// Can a user actively subscribe to the cursus?
    /// </summary>
    public bool? Enabled { get; set; } = null;
}
