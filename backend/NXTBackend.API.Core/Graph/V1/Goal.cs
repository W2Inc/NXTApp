using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Core.Graph.V1;

/// <summary>
/// A serialized XGraph V1 Goal
/// </summary>
public struct Goal
{
    /// <summary>
    /// The GUID of the goal
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the goal
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A description about the goal
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The state of the goal
    /// </summary>
    public TaskState State { get; set; }
}
