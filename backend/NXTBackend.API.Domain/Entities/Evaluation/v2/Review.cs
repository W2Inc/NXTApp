using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Evaluation.v2;

[Table("tbl_review")]
public class Review : BaseEntity
{
    // Columns //

    /// <summary>
    /// The type of review
    /// </summary>
    [Column("kind")]
    public ReviewKind Kind { get; set; }

    /// <summary>
    /// The current state of the review
    /// </summary>
    [Column("state")]
    public ReviewState State { get; set; }

    /// <summary>
    /// The git commit hash for the UserProject this review targeted.
    /// </summary>
    [Column("hash")]
    public string Hash { get; set; }

    /// <summary>
    /// The user doing the review.
    /// </summary>
    [Column("reviewer_id")]
    public Guid? ReviewerId { get; set; }

    /// <summary>
    /// The user project that this review is targeting.
    /// </summary>
    [Column("user_project_id")]
    public Guid UserProjectId { get; set; }

    // Relations //

    [ForeignKey(nameof(ReviewerId))]
    public virtual User? Reviewer { get; set; }

    [ForeignKey(nameof(UserProjectId))]
    public virtual UserProject UserProject { get; set; }

    /// <summary>
    /// Depending on the type of review, you may have multiple points of
    /// feedback.
    /// </summary>
    public virtual Collection<Feedback> Feedbacks { get; set; }
}
