// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

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

[Table("tbl_rubric")]
public class Rubric : BaseEntity
{
    public Rubric()
    {

    }
}