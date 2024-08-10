// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

/// <summary>
/// The patch request for updating a review
/// </summary>
public class FeebdackPostRequestDto : BaseRequestDto
{
    /// <summary>
    /// The feedback id the comment was made on
    /// </summary>
    [Required]
    public Guid ReviewId { get; set; }
}