// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

//pub name: String,
//    pub markdown: String,
//    pub description: String,
//    pub public: bool,
//    pub enabled: bool,
//    pub owner_id: Uuid,

public class GoalPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The title of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Name { get; set; }

    /// <summary>
    /// A small description of the event
    /// </summary>
    [Required, StringLength(128, MinimumLength = 4)]
    public string Description { get; set; }

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 128)]
    public string Markdown { get; set; }

    /// <summary>
    /// Is the rubric public / visible?
    /// </summary>
    [Required]
    public bool Public { get; set; }

    /// <summary>
    /// If false users can't use this rubric to perform a evaluations
    /// </summary>
    [Required]
    public bool Enabled { get; set; }
}