// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class RubricDO : BaseDO<Rubric>
{
    public RubricDO(Rubric rubric) : base(rubric)
    {
        Name = rubric.Name;
        Markdown = rubric.Markdown;
        Public = rubric.Public;
        Enabled = rubric.Enabled;
        Project = rubric.Project;
        Creator = rubric.Creator;
        GitInfo = rubric.GitInfo;
    }

    public string Name { get; set; }

    public string Markdown { get; set; }

    public bool Public { get; set; }

    public bool Enabled { get; set; }

    public ProjectDO? Project { get; set; }

    public UserDO? Creator { get; set; }

    public GitDO? GitInfo { get; set; }

    public static implicit operator RubricDO?(Rubric? entity) => entity is null ? null : new(entity);
}
