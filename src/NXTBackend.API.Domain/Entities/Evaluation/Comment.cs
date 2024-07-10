// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Evaluation;

/*
model Comment {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt
    markdown   String

    user_id String @db.Uuid
    user    User   @relation(fields: [user_id], references: [id])

    feedback_id String?   @db.Uuid
    Feedback    Feedback? @relation(fields: [feedback_id], references: [id])

    @@map("comment")
}
*/

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_comment")]
public class Comment : BaseEntity
{
    public Comment()
    {
        Markdown = string.Empty;
        UserId = Guid.Empty;
    }

    [Column("markdown")]
    public string Markdown { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [Column("feedback_id")]
    public Guid FeedbackId { get; set; }

    [ForeignKey(nameof(FeedbackId))]
    public virtual Feedback Feedback { get; set; } = null!;
}