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
        ReadNode(0);
    }

    private void ValidateChecksum()
    {
        const int C_CHECKSUM_LEN = 24;
        if (Length < C_CHECKSUM_LEN)
            throw new InvalidDataException("Invalid Data, unable to compute cheksum");

        long length = Length - (C_CHECKSUM_LEN + 1); // MD5 + NULL = 25 Bytes
        byte[] buffer = new byte[length];
        Read(buffer, 0, (int)length);

        string expectedChecksum = Convert.ToBase64String(MD5.HashData(buffer));
        Position = Length - (C_CHECKSUM_LEN + 1); // Position to read checksum

        string receivedChecksum = ReadCString() ?? string.Empty;
        if (expectedChecksum != receivedChecksum)
            throw new InvalidDataException("Bad Checksum");
        Position = 0; // Reset to the top to read the data
    }

    private void ReadNode(byte depth, Node? parent = null)
    {
        if (depth > Node.C_MAX_DEPTH)
            throw new InvalidOperationException($"Graph with a depth of: {depth} is too large!");

        short id = ReadInt16();
        short parentId = ReadInt16();
        short goalCount = ReadInt16();
        short childrenCount = ReadInt16();

        var node = new Node
        {
            Id = id,
            ParentId = parentId,
            Goals = [],
            Children = []
        };

        for (int i = 0; i < goalCount; i++)
        {
            node.Goals.Add(new Goal
            {
                Id = new(ReadCString() ?? throw new InvalidDataException("Bad Goal UUID"))
            });
        }

        if (depth == 0)
            Root = node;
        else
            parent?.Children.Add(node);

        ReadAlignment(8);
        for (int i = 0; i < childrenCount; i++)
            ReadNode((byte)(depth + 1), node);
    }
}
