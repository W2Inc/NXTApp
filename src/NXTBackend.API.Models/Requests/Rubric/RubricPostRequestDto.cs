// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Rubric;

/// <summary>
/// The post request for creating a new rubric
/// </summary>
public class RubricPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The name / title of the rubric
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string Name { get; set; }

    /// <summary>
    /// The markdown content of the rubric
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string Markdown { get; set; }

    /// <summary>
    /// Is the rubric public / visible?
    /// </summary>
    public bool Public { get; set; } = false;

    /// <summary>
    /// If false users can't use this rubric to perform a evaluations
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// This rubric applies to the project with this id
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// The git information (Url, branch, commit) of the rubric
    /// </summary>
    public GitInfoRequestDto GitInfo { get; set; }
}