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

    public void WriteData(List<GraphNode> nodes)
    {
        WriteHeader(new GraphHeader
        {
            Version = GraphFileVersion.Version1,
            NodeCount = (ushort)nodes.Count,
            GoalCount = (ushort)nodes.Sum(x => x.Goals.Count),
            NodeDepth = 6
        });

        foreach (var node in nodes)
            WriteNode(node.Id, node);
    }

    public void WriteNode(int id, GraphNode node)
    {
        Write(id);
        Write(node.Parent is null ? 0 : node.ParentId); // Write parent id, -1 if root
        Write(node.IsRoot);
        Write((ushort)node.Goals.Count);

        foreach (var goal in node.Goals)
        {
            WriteCString(goal.Name);
            Write(goal.GoalId);
        }
    }
}
