// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Domain.Entities.Users;

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
        Members = null!;
    }

    [Column("state")]
    public TaskState State { get; set; }

    [Column("project_id")]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    [Column("git_info_id")]
    public Guid? GitInfoId { get; set; }

    [ForeignKey(nameof(GitInfoId))]
    public virtual Git? GitInfo { get; set; }

    /// <summary>
    /// All the members that are part of this project.
    /// At most N members to what the project allows.
    /// </summary>
    public virtual ICollection<Member> Members { get; set; }

}
