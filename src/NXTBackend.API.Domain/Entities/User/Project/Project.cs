// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Review;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.User.Project;

/*
model UserProject {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt
    state      State    @default(INACTIVE)

    project       Project         @relation(fields: [project_id], references: [id])
    git_info      GitInfo?        @relation(fields: [git_info_id], references: [id])
    rubric        Rubric          @relation(fields: [rubric_id], references: [id])
    project_users ProjectMember[]
    reviews       Review[]

    rubric_id   String  @db.Uuid
    project_id  String  @db.Uuid
    git_info_id String? @db.Uuid

    @@map("user_project")
}
*/

/// <summary>
/// User projects are project "instances" / "sessions" for a project.
/// User's can partake in this session and complete it with the help of other members.
/// </summary>
[Table("tbl_user_project")]
public class UserProject : BaseEntity
{
    public UserProject()
    {
        State = TaskState.Inactive;
        ProjectId = Guid.Empty;
        Project = null!;
        GitInfoId = Guid.Empty;
        GitInfo = null!;
        RubricId = Guid.Empty;
        Rubric = null!;
        ProjectMembers = [];
    }

    [Column("state")]
    public TaskState State { get; set; }

    [Column("project_id")]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    [Column("git_info_id")]
    public Guid GitInfoId { get; set; }

    [ForeignKey(nameof(GitInfoId))]
    public virtual Git GitInfo { get; set; }

    [Column("rubric_id")]
    public Guid RubricId { get; set; }

    [ForeignKey(nameof(RubricId))]
    public virtual Rubric Rubric { get; set; }

    /// <summary>
    /// All the members that are part of this project.
    /// At most N members to what the project allows.
    /// </summary>
    public virtual ICollection<Member> ProjectMembers { get; set; }

}