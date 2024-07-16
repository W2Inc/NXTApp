// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Cursus;

public class CursusTrack
{
    [MaxLength(3)]
    public Guid[] Goals { get; set; } = [];

    [MaxLength(4)]
    public CursusTrack[] Next { get; set; } = [];
}

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class CursusPathPutRequestDto : BaseRequestDto
{
    /// <summary>
    /// The track of the cursus as in the path, may be null if the cursus is marked as dynamic
    /// </summary>
    [Required]
    public byte[] Track { get; set; } = null!;
}