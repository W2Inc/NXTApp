// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Models.Shared.Graph;

namespace NXTBackend.API.Models.Shared;

/// <summary>
/// Represents the entire track of a cursus for API requests and responses
/// </summary>
public class CursusTrackDTO : BaseRequestDTO
{
    /// <summary>
    /// Root node of the cursus track
    /// </summary>
    [Required]
    public TrackNodeDTO Root { get; set; }
}