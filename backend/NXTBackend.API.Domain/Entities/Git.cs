// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

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
        Name = string.Empty;
        Namespace = string.Empty;
        Url = string.Empty;
        Branch = string.Empty;
        ProviderType = GitProviderKind.Managed;
        OwnerType = OwnerKind.User;
    }

    [Column("git_name")]
    public string Name { get; set; }

    [Column("git_namespace")]
    public string Namespace { get; set; }

    [Column("git_url")]
    public string Url { get; set; }

    [Column("git_branch")]
    public string Branch { get; set; }

    [Column("git_provider")]
    public GitProviderKind ProviderType { get; set; }

    [Column("git_owner")]
    public OwnerKind OwnerType { get; set; }

    public virtual IEnumerable<Project> Projects { get; set; }

    public virtual IEnumerable<Rubric> Rubrics { get; set; }

    public virtual IEnumerable<UserProject> UserProjects { get; set; }
}
