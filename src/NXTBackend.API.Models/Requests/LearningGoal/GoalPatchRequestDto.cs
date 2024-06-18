// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// 
/// </summary>
public class GoalPatchRequestDto : BaseRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Public { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Enabled { get; set; }
}