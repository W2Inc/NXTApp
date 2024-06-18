// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities;

/*
model Project {
    id String @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid

    git_url    String  @unique
    git_branch String
    git_commit String?

    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    projects      Project[]
    rubrics       Rubric[]
    user_projects UserProject[]

    @@map("git_info")
}
*/

[Table("tbl_git")]
public class Git : BaseEntity
{
    public Git()
    {
        GitUrl = string.Empty;
        GitBranch = string.Empty;
        GitCommit = null;
    }

    [Required]
    [Column("git_url"), Url]
    public string GitUrl { get; set; }

    [Required]
    [Column("git_branch")]
    public string GitBranch { get; set; }

    [Column("git_commit")]
    public string? GitCommit { get; set; }
}