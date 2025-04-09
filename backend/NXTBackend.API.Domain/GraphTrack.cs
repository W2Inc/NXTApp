// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Domain;

/// <summary>
/// Defines a contract for graph track structures
/// </summary>
public interface IGraphTrack
{
    /// <summary>
    /// Collection of goal identifiers
    /// </summary>
    public Guid[] Goals { get; }

    /// <summary>
    /// Collection of next nodes in the graph
    /// </summary>
    public GraphNodeData[] Next { get; }
}

public record GraphNode(int Id, GraphNodeGoal[] Goals, GraphNode[] Children);
public record GraphNodeGoal(Guid Id, string Name, string Description, TaskState State);
public record GraphNodeData(Guid[] Goals, GraphNodeData[] Next);
