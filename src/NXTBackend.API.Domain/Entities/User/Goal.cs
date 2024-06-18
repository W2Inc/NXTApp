// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities;

/*
model LearningGoal {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    name        String @unique
    slug        String @unique
    markdown    String
    description String @db.VarChar(256)

    public     Boolean @default(false)
    enabled    Boolean @default(false)
    deprecated Boolean @default(false)

    owner_id String @db.Uuid
    owner    User   @relation("GoalCreator", fields: [owner_id], references: [id])

    projects      Project[]
    user_goals    UserGoal[]
    goal_vertices CursusVertex[] // This goal is part of these vertices

    @@map("learning_goal")
}
*/

[Table("tbl_user_goal")]
public class UserGoal : BaseEntity
{
    public UserGoal()
    {

    }
}