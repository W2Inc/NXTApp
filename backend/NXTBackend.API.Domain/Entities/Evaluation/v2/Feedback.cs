using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Evaluation.v2;

/// <summary>
/// Feedback for a given review object.
/// </summary>
[Table("tbl_feedback")]
public class Feedback : BaseEntity
{
    public Feedback()
    {
        Comments = new List<Comment>();
    }

    // Columns //

    [Column("kind")]
    public FeedbackKind Kind { get; set; }

    [Column("review_id")]
    public Guid ReviewId { get; set; }

    /// <summary>
    /// JSON data column that stores either annotation or correction data
    /// based on the Kind property
    /// </summary>
    [Column("feedback_data", TypeName = "jsonb")]
    protected string FeedbackDataJson { get; set; }

    // Relations //

    [ForeignKey(nameof(ReviewId))]
    public virtual Review Review { get; set; }

    /// <summary>
    /// Comment chain on this feedback
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; }

    // Non-mapped properties for working with the JSON data

    [NotMapped]
    public AnnotationData? AnnotationData
    {
        get => Kind is FeedbackKind.Annotation ?
            JsonSerializer.Deserialize<AnnotationData>(FeedbackDataJson) : null;
        set => FeedbackDataJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public CorrectionData? CorrectionData
    {
        get => Kind is FeedbackKind.Correction ?
            JsonSerializer.Deserialize<CorrectionData>(FeedbackDataJson) : null;
        set => FeedbackDataJson = JsonSerializer.Serialize(value);
    }
}

/// <summary>
/// Data structure for annotation-type feedback
/// </summary>
public class AnnotationData
{
    /// <summary>
    /// List of annotations for specific code sections
    /// </summary>
    public List<AnnotationItem> Items { get; set; } = new();

    /// <summary>
    /// Inline comments associated with each annotation
    /// </summary>
    public List<AnnotationComment> Comments { get; set; } = new();
}

/// <summary>
/// Individual annotation within a file
/// </summary>
public class AnnotationItem
{
    /// <summary>
    /// Starting line number
    /// </summary>
    public int StartLine { get; set; }

    /// <summary>
    /// Ending line number
    /// </summary>
    public int EndLine { get; set; }
}

/// <summary>
/// Comments on specific annotations
/// </summary>
public class AnnotationComment
{
    /// <summary>
    /// Unique identifier for this comment
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// References the annotation this comment belongs to
    /// </summary>
    public string AnnotationId { get; set; }

    /// <summary>
    /// User ID of commenter
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Comment content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// When the comment was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Data structure for correction-type feedback
/// </summary>
public class CorrectionData
{
    /// <summary>
    /// S3 path to the corrected file
    /// </summary>
    public string S3FilePath { get; set; }

    /// <summary>
    /// Original file name
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Comments on the whole file correction
    /// </summary>
    public List<CorrectionComment> Comments { get; set; } = new();
}

/// <summary>
/// Comments on whole file corrections
/// </summary>
public class CorrectionComment
{
    /// <summary>
    /// Unique identifier for this comment
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// User ID of commenter
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Comment content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// When the comment was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
