// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Domain;

/// <summary>
/// Defines a contract for the basic structure of a cursus track node,
/// focusing on goal identifiers and subsequent nodes.
/// </summary>
public class CursusTrack
{
    /// <summary>
    /// Collection of goal identifiers for the current node/step.
    /// </summary>
    [Required, MinLength(1), MaxLength(3)]
    public Guid[] Goals { get; set; }

    /// <summary>
    /// Collection of next nodes in the graph, representing the subsequent steps.
    /// </summary>
    [Required, MaxLength(4)]
    public CursusTrack[] Next { get; set; }
}
