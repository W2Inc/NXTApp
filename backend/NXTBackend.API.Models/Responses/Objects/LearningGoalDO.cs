// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

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

    public string Name { get; set; }

    public string Slug { get; set; }

    public string Markdown { get; set; }

    public string Description { get; set; }

    public SimpleUserDO? Creator { get; set; }

    public static implicit operator LearningGoalDO?(LearningGoal? entity) => entity is null ? null : new(entity);
}
