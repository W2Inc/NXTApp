// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Review;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(FeedbackWithAnnotationDTO), nameof(FeedbackKind.Annotation))]
[JsonDerivedType(typeof(FeedbackWithCorrectionDTO), nameof(FeedbackKind.Correction))]
public abstract class FeedbackPostRequestDTO : BaseRequestDTO
{
    [JsonIgnore, JsonPropertyName("kind")]
    public FeedbackKind Kind { get; set; }

    [Required, StringLength(2048, MinimumLength = 1)]
    [Description("The establishing comment for this feedback.")]
    public string Comment { get; set; }

    [Required, StringLength(2048, MinimumLength = 1)]
    [Description("Full file path, must match that of the remote.")]
    public string File { get; set; }

    /// <summary>
    /// Optionally set who published the comment, if not set current authenticated
    /// user is assumed to have publish the comment instead.
    ///
    /// Requires priviliges.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? UserId { get; set; } = null;
}

public class FeedbackWithAnnotationDTO : FeedbackPostRequestDTO
{
    [Required]
    [Description("The start line for the annotation")]
    public int Start { get; set; }

    [Required]
    [Description("The end line for the annotation")]
    public int End { get; set; }
}

public class FeedbackWithCorrectionDTO : FeedbackPostRequestDTO
{
    [Description(@"
Url to the corrected / annotated version of this file.
Server will *not* create this URL for you, you must provide a valid URL
    ")]
    public string FileCorrectionUrl { get; set; }
}
