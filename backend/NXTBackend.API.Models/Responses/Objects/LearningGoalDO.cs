// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class LearningGoalDO : BaseObjectDO<LearningGoal>
{
    public LearningGoalDO(LearningGoal learningGoal) : base(learningGoal)
    {
        Name = learningGoal.Name;
        Slug = learningGoal.Slug;
        Markdown = learningGoal.Markdown;
        Description = learningGoal.Description;
        Creator = learningGoal.Creator;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Slug { get; set; }

    [Required]
    public string Markdown { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public MinimalUserDTO? Creator { get; set; }

    public static implicit operator LearningGoalDO?(LearningGoal? entity) => entity is null ? null : new(entity);
}
