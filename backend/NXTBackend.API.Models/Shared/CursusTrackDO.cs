// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain;
using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Shared;

/// <summary>
/// Request and Response obkect
/// </summary>
public class CursusTrackDO : BaseRequestDTO, IGraphTrack
{
    [Required, MinLength(1), MaxLength(3)]
    public Guid[] Goals { get; set; }

    [Required, MinLength(0), MaxLength(4)]
    public GraphNodeData[] Next { get; set; }
}
