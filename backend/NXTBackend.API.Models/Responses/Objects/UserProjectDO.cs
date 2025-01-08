// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class UserProjectDO : BaseObjectDO<UserProject>
{
    public UserProjectDO(UserProject userProject) : base(userProject)
    {
        State = userProject.State;
        Project = userProject.Project;
        GitInfo = userProject.GitInfo;
        // Rubric = userProject.Rubric;
    }

    [Required]
    public TaskState State { get; set; }

    [Required]
    public virtual ProjectDO? Project { get; set; }

    [Required]
    public virtual GitDO? GitInfo { get; set; }

    // public virtual RubricDO? Rubric { get; set; }

    public static implicit operator UserProjectDO?(UserProject? entity) => entity is null ? null : new(entity);
}
