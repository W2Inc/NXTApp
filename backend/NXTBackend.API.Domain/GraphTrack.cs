// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Domain;

public record GraphNode(int Id, GraphNodeGoal[] Goals, GraphNode[] Children);
public record GraphNodeGoal(Guid Id, string Name, string Description, TaskState State);
