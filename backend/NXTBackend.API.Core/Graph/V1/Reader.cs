using System.Security.Cryptography;
using Snowberry.IO.Reader;

namespace NXTBackend.API.Core.Graph.V1;

public sealed class Reader(Stream? stream) : EndianStreamReader(stream, null, 0)
{
    public Header Header { get; } = new();
    public Node? Root { get; private set; } = null;

    public void Deserialize()
    {
        ValidateChecksum();
        Header.Read(this);


    }

    private void ValidateChecksum()
    {
        if (this.ActualLength < 24)
            throw new InvalidDataException("Invalid Data, unable to compute cheksum");

        // MD5 + NULL = 25 Bytes
        var offset = Position = ActualLength - (24 + 1);
        var bufferSlice = new ReadOnlySpan<byte>(this.Buffer, 0, (int)offset);

        // Get the Hash
        var receivedChecksum = ReadCString() ?? string.Empty;
        var expectedChecksum = Convert.ToBase64String(MD5.HashData(bufferSlice));
        if (expectedChecksum != receivedChecksum)
            throw new InvalidDataException("Bad Checksum");
        Position = 0; // Reset to the top to read the data
    }

    private void ReadNode(byte depth, Node? node = null)
    {
        if (depth > Node.C_MAX_DEPTH)
            throw new InvalidOperationException($"Graph with a depth of: {depth} is too large!");

        var id = ReadInt16();
        var parentId = ReadInt16();
        var goalCount = ReadInt16();
        var childrenCount = ReadInt16();

        // if (node.Goals.Count > Node.C_MAX_GOALS)
        //     throw new InvalidOperationException("Node can't have more than 4 goals.");
        // if (node.Children.Count > Node.C_MAX_NODES)
        //     throw new InvalidOperationException("Node can't have more than 4 children.");

        // Write(node.Id);
        // Write(node.ParentId);
        // Write((ushort)node.Goals.Count); // GoalCount
        // Write((ushort)node.Children.Count); // ChildrenCount

        // foreach (var goal in node.Goals)
        // {
        //     // WriteCString(goal.Name);
        //     Write(goal.Id);
        //     // Write((int)goal.State);
        //     // WriteCString(goal.Description);
        // }

        // WritePadding(8);
        // foreach (var child in node.Children)
        //     WriteNode(node.Id, child, depth++);
    }
}
