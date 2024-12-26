using Snowberry.IO.Reader;
using Snowberry.IO.Writer;

namespace NXTBackend.API.Core.Graph.V1;

/// <summary>
/// XGraphV1 header
/// </summary>
public class Header
{
    /// <summary>
    /// Version
    /// </summary>
    public const Version C_VERSION = Version.XGraphV1;

    /// <summary>
    /// Magic: XGRAPH in Ascii
    /// </summary>
    public const ulong C_MAGIC = 0x485041524758;

    /// <summary>
    /// The amount of total nodes in the graph.
    /// </summary>
    public ushort NodeCount;

    /// <summary>
    /// The amount of goals referenced in the graph.
    /// </summary>
    public ushort GoalCount;

    public void Read(BaseEndianReader reader)
    {
        int version = reader.ReadInt32();
        if (version is not (int)Version.XGraphV1)
            throw new InvalidDataException("Bad XGraph Version");

        reader.ReadAlignment(16);
        if (reader.ReadULong() != C_MAGIC)
            throw new InvalidDataException("Bad Magic");

        NodeCount = reader.ReadUInt16();
        GoalCount = reader.ReadUInt16();
        reader.ReadAlignment(16);
    }

    public void Write(EndianStreamWriter writer)
    {
        writer.Write((int)C_VERSION);
        writer.WritePadding(16);
        writer.Write(C_MAGIC);
        writer.Write(NodeCount);
        writer.Write(GoalCount);
        writer.WritePadding(16);
    }
}
