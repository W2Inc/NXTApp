using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Shared;

/// <summary>
/// Represents a goal within a track node
/// </summary>
public class TrackGoalDTO
{
    /// <summary>
    /// The unique identifier of this goal
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The display name of this goal
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The current state of this goal
    /// </summary>
    public TaskState State { get; set; }
}