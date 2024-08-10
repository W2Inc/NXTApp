using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTBackend.API.Core.Graph.Meta;
using Snowberry.IO.Writer;

namespace NXTBackend.API.Core.Graph;

public class GraphWriter : EndianStreamWriter
{
    public GraphWriter(Stream stream) : base(stream, true)
    {

    }

    public void WriteHeader(GraphHeader header) => header.Write(this);

    public void WriteData(GraphNode node)
    {
        // Nodes are a tree like structure, they have a next list of nodes who also have a next list of nodes and so on
        // the depth of this must not exceed 255 otherwise it is too large.
        // Furthermore there should be no more than 65535 nodes in the graph.
        // Each Node can't have more than 4 goals.
        // Each goal name can't exceed 255 characters.

        WriteHeader(new GraphHeader
        {
            Version = GraphFileVersion.XGraphV1,
            NodeCount = GetTotalNodeCount(node),
            GoalCount = GetTotalGoalCount(node)
        });

        foreach (var child in node.Children)
        {
            byte depth = 0;
            if (child is null)
                continue;

            WriteNode(node.Id, child, depth);
        }
    }

    private void WriteNode(ushort id, GraphNode node, byte depth)
    {
        if (depth > GraphNode.C_MAX_DEPTH)
            throw new InvalidOperationException($"Graph with a depth of: {depth} is too large!");

        Write(id);
        Write(node.ParentId);
        Write(node.ParentId == 0); // isRoot

        if (node.Goals.Count > GraphNode.C_MAX_GOALS)
            throw new InvalidOperationException("Node can't have more than 4 goals.");
        if (node.Children.Count > GraphNode.C_MAX_NODES)
            throw new InvalidOperationException("Node can't have more than 4 children.");

        Write((ushort)node.Goals.Count); // GoalCount
        Write((ushort)node.Children.Count); // ChildrenCount

        foreach (var goal in node.Goals)
        {
            WriteCString(goal.Name);
            Write(goal.GoalId);
        }

        WritePadding(8);
        foreach (var child in node.Children)
            WriteNode(node.Id, child, depth++);
    }

    private ushort GetTotalNodeCount(GraphNode node)
    {
        return (ushort)(1 + node.Children.Sum(x => GetTotalNodeCount(x)));
    }

    private ushort GetTotalGoalCount(GraphNode node)
    {
        return (ushort)(node.Goals.Count + node.Children.Sum(x => GetTotalGoalCount(x)));
    }
}
