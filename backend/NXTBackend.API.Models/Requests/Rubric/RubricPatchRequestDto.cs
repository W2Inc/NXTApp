// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Rubric;

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class RubricPatchRequestDto : BaseRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Public { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool? Enabled { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public Guid? ProjectId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public GitInfoRequestDto? GitInfoId { get; set; }
}