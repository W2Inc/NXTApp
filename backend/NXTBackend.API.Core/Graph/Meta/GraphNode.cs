using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXTBackend.API.Core.Graph.Meta;

public struct GoalEntry
{
    public string Name { get; set; }

    public Guid GoalId { get; set; }
}

public class GraphNode()
{
    public const ushort C_MAX_NODES = 4;
    public const ushort C_MAX_GOALS = 3;
    public const byte C_MAX_DEPTH = byte.MaxValue;

    public ushort Id { get; set; }

    public ushort ParentId { get; set; }

    public bool IsRoot { get; set; }

    public ushort GoalCount { get; set; }

    public ushort ChildrenCount { get; set; }

    public List<GoalEntry> Goals { get; set; } = [];

    public List<GraphNode> Children { get; set; } = [];
}
