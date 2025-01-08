// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities.Evaluation;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class RubricDO : BaseObjectDO<Rubric>
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

    [Required]
    public string Name { get; set; }

    [Required]
    public string Markdown { get; set; }

    [Required]
    public bool Public { get; set; }

    [Required]
    public bool Enabled { get; set; }

    [Required]
    public ProjectDO? Project { get; set; }

    [Required]
    public UserDO? Creator { get; set; }

    [Required]
    public GitDO? GitInfo { get; set; }

    public static implicit operator RubricDO?(Rubric? entity) => entity is null ? null : new(entity);
}
