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

public class GraphNode(GraphNode? Parent = null)
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public bool isRoot { get; set; }

    public ushort GoalCount { get; set; }

    public GraphNode? Parent { get; set; } = Parent;

    public bool IsRoot => Parent == null;

    public List<GoalEntry> Goals { get; set; } = [];

    public List<GraphNode> Next { get; set; } = [];
}
