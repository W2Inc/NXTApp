using System.Buffers.Text;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Snowberry.IO.Writer;

namespace NXTBackend.API.Core.Graph.V1;

public sealed class Writer(Stream stream) : EndianStreamWriter(stream, true)
{
    public void Serialize(Node node)
    {
        var header = new Header()
        {
            GoalCount = GetTotalNodeCount(node),
            NodeCount = GetTotalNodeCount(node)
        };

        header.Write(this);
        WriteNode(-1, node, 0);
        FinalizeChecksum();
    }

    private void WriteNode(short parentId, Node node, byte depth)
    {
        if (depth > Node.C_MAX_DEPTH)
            throw new InvalidOperationException($"Graph with a depth of: {depth} is too large!");
        if (node.Goals.Count > Node.C_MAX_GOALS)
            throw new InvalidOperationException("Node can't have more than 4 goals.");
        if (node.Children.Count > Node.C_MAX_NODES)
            throw new InvalidOperationException("Node can't have more than 4 children.");

        Write(node.Id);
        Write(node.ParentId);
        Write((ushort)node.Goals.Count); // GoalCount
        Write((ushort)node.Children.Count); // ChildrenCount

        foreach (var goal in node.Goals)
        {
            // WriteCString(goal.Name);
            Write(goal.Id);
            Write((int)goal.State);
            // WriteCString(goal.Description);
        }

        WritePadding(8);
        foreach (var child in node.Children)
            WriteNode(node.Id, child, depth++);
    }

    private void FinalizeChecksum() => WriteCString(Convert.ToBase64String(MD5.HashData(this.OutStream)));

    private ushort GetTotalNodeCount(Node node)
    {
        return (ushort)(1 + node.Children.Sum(x => GetTotalNodeCount(x)));
    }

    private ushort GetTotalGoalCount(Node node)
    {
        return (ushort)(node.Goals.Count + node.Children.Sum(x => GetTotalGoalCount(x)));
    }
}
