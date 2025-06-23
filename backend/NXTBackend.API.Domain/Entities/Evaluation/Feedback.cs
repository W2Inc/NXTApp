using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Evaluation;

/// <summary>
/// Feedback for a given review object.
/// </summary>
[Table("tbl_feedback")]
public class Feedback : BaseEntity
{
    // Columns //

    [Column("kind")]
    public FeedbackKind Kind { get; set; }

    [Column("review_id")]
    public Guid ReviewId { get; set; }

    // Relations //

    [ForeignKey(nameof(ReviewId))]
    public virtual Review Review { get; set; }

    // Non-mapped properties for working with the JSON data

    // [NotMapped]
    // public AnnotationData? AnnotationData
    // {
    //     get => Kind is FeedbackKind.Annotation ?
    //         JsonSerializer.Deserialize<AnnotationData>(FeedbackDataJson) : null;
    //     set => FeedbackDataJson = JsonSerializer.Serialize(value);
    // }

    // [NotMapped]
    // public CorrectionData? CorrectionData
    // {
    //     get => Kind is FeedbackKind.Correction ?
    //         JsonSerializer.Deserialize<CorrectionData>(FeedbackDataJson) : null;
    //     set => FeedbackDataJson = JsonSerializer.Serialize(value);
    // }
}
