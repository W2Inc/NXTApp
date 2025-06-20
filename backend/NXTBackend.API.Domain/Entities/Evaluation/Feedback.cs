﻿// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Evaluation;

/*
model Feedback {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    comments  Comment[]
    review_id String    @db.Uuid
    review    Review    @relation(fields: [review_id], references: [id])

    @@unique([review_id], name: "unique_feedback_review_id")
    @@map("feedback")
}
*/

// ============================================================================

/// <summary>
/// Feedback is targeted comments on some object.
///
/// E.g: A review on a project may contain multiple feedbacks each feedback may have multiple comments
/// </summary>
[Table("tbl_feedback")]
public class Feedback : BaseEntity
{
    public Feedback()
    {
        ReviewId = Guid.Empty;
        Review = null!;
        Comments = [];
    }

    [Column("review_id")] // Add unqiue constraint
    public Guid ReviewId { get; set; }

    [ForeignKey(nameof(ReviewId))]
    public virtual Review Review { get; set; }

    public virtual IEnumerable<Comment> Comments { get; set; }
}
