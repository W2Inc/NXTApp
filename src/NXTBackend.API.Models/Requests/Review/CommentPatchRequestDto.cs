// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

/// <summary>
/// The post request for creating a new review
/// </summary>
public class CommentPatchRequestDto : BaseRequestDto
{
    /// <summary>
    /// The markdown content of the comment
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 1)]
    public string Markdown { get; set; }
}