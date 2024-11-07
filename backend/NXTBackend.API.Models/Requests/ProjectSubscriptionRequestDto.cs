using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests;

public class ProjectSubscriptionRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// Regarding which goal ...
    /// </summary>
    [Required]
    public Guid UserGoalId { get; set; }

    /// <summary>
    /// Which project is the user subscribing to?
    /// </summary>
    [Required]
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Which rubric is being used for the project
    /// </summary>
    [Required]
    public Guid RubricId { get; set; }
}
