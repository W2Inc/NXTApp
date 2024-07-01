// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities;

/*
model CursusVertex {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    cursus_id String @db.Uuid
    cursus    Cursus @relation("CursusVertices", fields: [cursus_id], references: [id])

    // TODO: Not just one goal but MANY! 
    //goal_id String       @db.Uuid
    //goal    LearningGoal @relation(fields: [goal_id], references: [id])
    goals      LearningGoal[]
    user_goals UserGoal[]

    parent_id String?        @map("parent_id") @db.Uuid
    parent    CursusVertex?  @relation("children", fields: [parent_id], references: [id])
    children  CursusVertex[] @relation("children")

    @@map("cursus_vertex")
}
*/

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_cursus_vertex")]
public class CursusVertex : BaseEntity
{
    public CursusVertex()
    {
        CursusId = Guid.Empty;
        Cursus = null!;
        //Goals = [];
        //UserGoals = [];
        ParentId = null;
        Parent = null;
        //Children = [];
    }

    [Column("parent_id")]
    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public virtual CursusVertex? Parent { get; set; }

    [Column("cursus_id")]
    public Guid? CursusId { get; set; }

    [ForeignKey(nameof(CursusId))]
    public virtual Cursus? Cursus { get; set; }

    //[JsonIgnore]
    //public virtual IEnumerable<LearningGoal> Goals { get; set; }

    //[JsonIgnore]
    //public virtual IEnumerable<UserGoal> UserGoals { get; set; }

    //[JsonIgnore]
    //public virtual IEnumerable<CursusVertex> Children { get; set; }
}