// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

public class ReviewPostRequestDTO : BaseRequestDTO
{
    [Required]
    public ReviewKind Kind { get; set; }

    [Required]
    public Guid RubricId { get; set; }

    [Required]
    public Guid UserProjectId { get; set; }

    [Required]
    public Guid? ReviewerId { get; set; } = null;
}
