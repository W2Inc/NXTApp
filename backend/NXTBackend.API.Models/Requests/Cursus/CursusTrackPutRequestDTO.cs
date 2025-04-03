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
public class CursusTrackPutRequestDTO : BaseRequestDTO
{
    [Required, MinLength(1), MaxLength(3)]
    public Guid[] Goals { get; set; }

    [Required, MinLength(0), MaxLength(4)]
    public CursusTrackPutRequestDTO[] Next { get; set; }
}
