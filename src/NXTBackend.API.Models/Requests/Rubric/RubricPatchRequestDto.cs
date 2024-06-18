// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class RubricPatchRequestDto : BaseRequestDto
{
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; }

    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; }

    public bool? Public { get; set; } = false;

    public bool? Enabled { get; set; } = false;

    public Guid? ProjectId { get; set; }

    public Guid? GitInfoId { get; set; }
}