﻿// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Evaluation;

/*
model Review {
    id String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid

    kind       ReviewType  @default(PERSONAL)
    state      ReviewState @default(PENDING)
    created_at DateTime    @default(now())
    updated_at DateTime    @default(now()) @updatedAt

    // user_id String @db.Uuid
    // user    User   @relation("Rubriced", fields: [user_id], references: [id])

    reviewer_id String? @db.Uuid
    reviewer    User?   @relation("Rubricer", fields: [reviewer_id], references: [id])

    rubric_id String @db.Uuid
    rubric    Rubric @relation(fields: [rubric_id], references: [id])

    // The project instance this review is for
    user_project_id String      @db.Uuid
    user_project    UserProject @relation(fields: [user_project_id], references: [id])

    feedback_id String?   @db.Uuid
    feedback    Feedback?
    validated   Boolean   @default(false)

    // Which objectives have been validated by the reviewer
    //validated_objectives    Objective[]

    @@map("review")
}
*/

[Table("tbl_review")]
public class Review : BaseEntity
{
    public Review()
    {
        Kind = ReviewKind.Self;
        State = ReviewState.Pending;
        Validated = false;

        ReviewerId = null;
        Reviewer = null;

        RubricId = null;
        Rubric = null;

        FeedbackId = null;
        Feedback = null;

        UserProjectId = null;
        UserProject = null;
    }

    [Column("name"), Required]
    public ReviewKind Kind { get; set; }

    /// <summary>
    /// A short description of the Cursus.
    /// </summary>
    [Column("description"), StringLength(256)]
    public ReviewState State { get; set; }

    /// <summary>
    /// Does this review indicate a successful completion of the project?
    /// 
    /// In most cases the reviewer is prompted to indicate wether the review
    /// passes or fails. For instance if the quality of the project was not sufficient.
    /// </summary>
    public bool Validated { get; set; }

    /// <summary>
    /// The user doing the review.
    /// </summary>
    [Column("reviewer_id")]
    public Guid? ReviewerId { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public virtual User? Reviewer { get; set; }

    /// <summary>
    /// The rubric that is being used for the review.
    /// </summary>
    [Column("rubric_id")]
    public Guid? RubricId { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public virtual Rubric? Rubric { get; set; }

    /// <summary>
    /// The feedback thread that is being used for the review.
    /// </summary>
    [Column("feedback_id")]
    public Guid? FeedbackId { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public virtual Feedback? Feedback { get; set; }

    /// <summary>
    /// The user project that this review is targeting.
    /// </summary>
    [Column("user_project_id")]
    public Guid? UserProjectId { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public virtual UserProject? UserProject { get; set; }
}