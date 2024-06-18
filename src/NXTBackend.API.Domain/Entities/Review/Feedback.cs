using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities;

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

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_feedback")]
public class Feedback : BaseEntity
{
    public Feedback()
    {
        //ReviewId = Guid.Empty;
        Comments = null!;
        //Review = null!;
    }

    [ForeignKey(nameof(Comment))]
    public virtual ICollection<Comment> Comments { get; set; }

    //[Column("review_id")] // Add unqiue constraint
    //public Guid ReviewId { get; set; }

    //[ForeignKey(nameof(ReviewId))]
    //public virtual Review Review { get; set; }
}