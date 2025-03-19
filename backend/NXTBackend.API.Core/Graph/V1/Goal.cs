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
    /// The state of the goal
    /// </summary>
    public TaskState State { get; set; }
}
