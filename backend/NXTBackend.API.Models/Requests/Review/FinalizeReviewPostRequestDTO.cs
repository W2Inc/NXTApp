// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

/// <summary>
/// DTO To Finalize
/// </summary>
public class ReviewFinalizePostRequestDTO : BaseRequestDTO
{
    [Required]
    public Guid UserProjectId { get; set; }

    [Required]
    public IEnumerable<FeedbackPostRequestDTO> Feedback { get; set; } = [];
}
