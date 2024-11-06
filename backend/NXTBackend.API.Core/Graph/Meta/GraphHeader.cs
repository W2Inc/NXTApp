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
    /// Magic number to identify the file.
    /// </summary>
    public const ulong C_MAGIC = 0xB0B0BEBAFECA;

    public bool Read(BaseEndianReader reader)
    {
        int version = reader.ReadInt32();
        if (version is not (int)GraphFileVersion.XGraphV1)
            return false;
        Version = (GraphFileVersion)version;

        // Account for padding in the header
        reader.ReadAlignment(16);
        if (reader.ReadULong() != C_MAGIC)
            return false;

        NodeCount = reader.ReadUInt16();
        GoalCount = reader.ReadUInt16();
        reader.ReadAlignment(16);
        return true;
    }

    public void Write(EndianStreamWriter writer)
    {
        writer.Write((int)Version);
        writer.WritePadding(16);
        writer.Write(C_MAGIC);  // ulong -> 8 bytes
        writer.Write(NodeCount); // 4 bytes
        writer.Write(GoalCount); // 4 bytes
        writer.WritePadding(16);
    }
}
