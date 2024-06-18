// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// The patch request for updating a review
/// </summary>
public class CommentPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The markdown content of the comment
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 1)]
    public string Markdown { get; set; }

    /// <summary>
    /// The user id of the user that made the comment
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The feedback id the comment was made on
    /// </summary>
    public Guid FeedbackId { get; set; }
}