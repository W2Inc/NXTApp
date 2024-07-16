using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowberry.IO.Common;
using Snowberry.IO.Extensions;
using Snowberry.IO.Reader;
using Snowberry.IO.Writer;

namespace NXTBackend.API.Core.Graph.Meta;

public class GraphHeader
{
    /// <summary>
    /// Graph format version
    /// </summary>
    public GraphFileVersion Version;

    /// <summary>
    /// The amount of total nodes in the graph.
    /// </summary>
    public ushort NodeCount;

    /// <summary>
    /// The amount of goals referenced in the graph.
    /// </summary>
    public ushort GoalCount;

    /// <summary>
    /// The depth of the graph. A.k.a. the amount of nodes in the longest path.
    /// </summary>
    public ushort NodeDepth;

    /// <summary>
    /// Magic number to identify the file.
    /// </summary>
    public const long C_MAGIC = 0x7765616B636F6465; // "weakcode"

    public bool Read(BaseEndianReader reader)
    {
        int version = reader.ReadInt32();
        if (version is not (int)GraphFileVersion.Version1)
            return false;
        Version = (GraphFileVersion)version;

        // SKip 4 bytes for padding
        reader.Position += 4;
        if (reader.ReadLong() != C_MAGIC)
            return false;

        NodeCount = reader.ReadUInt16();
        GoalCount = reader.ReadUInt16();
        NodeDepth = reader.ReadUInt16();

        // Skip 10 bytes for padding
        reader.Position += 10;
        return true;
    }

    public void Write(EndianStreamWriter writer)
    {
        writer.Write((int)Version);
        writer.WritePadding(4);
        writer.Write(C_MAGIC);
        writer.Write(NodeCount);
        writer.Write(GoalCount);
        writer.Write(NodeDepth);
        writer.WritePadding(10);
    }
}
