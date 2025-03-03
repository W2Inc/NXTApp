// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

/// <summary>
/// The patch request for updating a review
/// </summary>
public class ReviewPatchRequestDto : BaseRequestDTO
{
    /// <summary>
    ///
    /// </summary>
    public ReviewState? State { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Guid? FeedbackId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Guid? ReviewerId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool? Validated { get; set; }
}
