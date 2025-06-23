// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

/// <summary>
/// The patch request for updating a review
/// </summary>
public class CommentPutRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// The markdown content of the comment
    /// </summary>
    [Required, StringLength(2048, MinimumLength = 1)]
    public string Markdown { get; set; }

    /// <summary>
    /// The user id of the user that made the comment, requires admin
    /// permissions
    /// </summary>
    public Guid? UserId { get; set; }
}
