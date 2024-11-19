using NXTBackend.API.Core.Graph.Meta;
using Snowberry.IO.Reader;

namespace NXTBackend.API.Core.Graph;
public sealed class GraphReader(Stream? stream) : EndianStreamReader(stream, null, 0)
{
    public GraphHeader Header { get; } = new();
    public GraphNode? RootNode { get; private set; } = null;

    public void ReadData()
    {
        if (!Header.Read(this))
            throw new InvalidDataException("Invalid graph file");
        ReadNode(0, null); // Start reading nodes with depth 0 and no parent
    }

    private void ReadNode(byte depth, GraphNode? parent)
    {
        if (depth > GraphNode.C_MAX_DEPTH)
            throw new InvalidDataException($"Graph with a depth of: {depth} is too large");

        ushort id = ReadUInt16();
        ushort parentId = ReadUInt16();
        bool isRoot = ReadBool();
        ushort goalCount = ReadUInt16();
        ushort childrenCount = ReadUInt16();

        if (goalCount > GraphNode.C_MAX_GOALS)
            throw new InvalidDataException($"Node can't have more than {GraphNode.C_MAX_GOALS} goals");
        if (childrenCount > GraphNode.C_MAX_NODES)
            throw new InvalidDataException($"Node can't have more than {GraphNode.C_MAX_NODES} children");

        var node = new GraphNode
        {
            Id = id,
            ParentId = parentId,
            IsRoot = parent is null || isRoot,
            GoalCount = goalCount,
            ChildrenCount = childrenCount
        };

        RootNode = parent is null ? node : RootNode;
        for (int i = 0; i < goalCount; i++)
        {
            string? goalName = ReadCString();
            if (string.IsNullOrEmpty(goalName))
                throw new InvalidDataException($"Node: {id:guid} has a goal with an invalid name");

            var goalId = ReadGuid();
            node.Goals.Add(new GoalEntry
            {
                Name = goalName,
                GoalId = goalId
            });
        }

        parent?.Children.Add(node);
        ReadAlignment(8);
        for (int i = 0; i < childrenCount; i++)
            ReadNode((byte)(depth + 1), node); // Read child nodes and increase the depth
    }
}
