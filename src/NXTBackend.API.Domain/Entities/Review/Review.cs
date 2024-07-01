// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities;

/*
model Project {
    id String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid

    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    name          String  @unique
    description   String  @db.VarChar(256)
    markdown      String
    slug          String  @unique
    thumbnail_url String?
    public        Boolean @default(false)
    enabled       Boolean @default(false)
    deprecated    Boolean @default(false)
    max_members   Int     @default(1)

    // Objectives that are part of this project that get selected on review
    // For example: "Learn to use git", "Learn to use github", "Learn to use gitlab"
    //objectives    Objective[]
    rubrics       Rubric[]
    goals         LearningGoal[]
    user_projects UserProject[]

    git_info_id String?  @db.Uuid
    git_info    GitInfo? @relation(fields: [git_info_id], references: [id])

    owner_id String @db.Uuid
    owner    User   @relation("ProjectCreator", fields: [owner_id], references: [id])
    tags     Tag[]  @relation("ProjectTags")

    @@map("project")
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
    public virtual UserProject.UserProject? UserProject { get; set; }
}