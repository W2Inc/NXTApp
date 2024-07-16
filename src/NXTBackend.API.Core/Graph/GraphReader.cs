using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTBackend.API.Core.Graph.Meta;
using Snowberry.IO.Common.Reader;
using Snowberry.IO.Reader;

namespace NXTBackend.API.Core.Graph;
public sealed class GraphReader : EndianStreamReader
{
    public GraphHeader Header { get; }
    public Dictionary<int, GraphNode> Nodes { get; } = new();

    /// <inheritdoc/>
    public GraphReader(Stream? stream) : base(stream, null, 0)
    {
        Header = new();
    }

    public void ReadHeader()
    {
        if (!Header.Read(this))
            throw new InvalidDataException("Invalid graph file.");
    }

    public void ReadData()
    {
        for (int i = 0; i < Header.NodeCount; i++)
            ReadNode();
    }

    private void ReadNode()
    {
        int id = ReadInt32();
        int parentId = ReadInt32();
        bool isRoot = ReadBool();
        ushort goalCount = ReadUInt16();

        GraphNode? parent = null;
        if (!isRoot)
        {
            if (!Nodes.TryGetValue(parentId, out parent))
                throw new InvalidDataException($"Parent node with ID {parentId} not found.");
        }

        GraphNode node = new(isRoot ? null : parent!);
        for (int i = 0; i < goalCount; i++)
        {
            string? goalName = ReadCString();
            if (string.IsNullOrEmpty(goalName))
                throw new InvalidDataException($"Node: {id}: Has a goal with an invalid name.");

            var goalId = ReadGuid();
            node.Goals.Add(new GoalEntry {
                Name = goalName,
                GoalId = goalId
            });
        }

        Nodes.Add(id, node);
    }
}
