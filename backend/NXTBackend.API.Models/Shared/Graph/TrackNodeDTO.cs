using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Shared.Graph;

/// <summary>
/// Represents a node in the cursus track
/// </summary>
public class TrackNodeDTO
{
    /// <summary>
    /// Simply the index of the node, can be serial.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Goals that this node represents. Multiple goals indicate a choice.
    /// </summary>
    [Required, MinLength(1), MaxLength(3)]
    public TrackGoalDTO[] Goals { get; set; }
    
    /// <summary>
    /// Child nodes that follow this node in the track
    /// </summary>
    [Required, MinLength(1), MaxLength(3)]
    public TrackNodeDTO[] Children { get; set; } = [];
}