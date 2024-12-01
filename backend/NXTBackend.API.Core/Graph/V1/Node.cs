namespace NXTBackend.API.Core.Graph.V1;

public class Node()
{
    /// <summary>
    /// The max amount of nodes a node can have
    /// </summary>
    public const ushort C_MAX_NODES = 4;

    /// <summary>
    /// The max amount of goals a node can have
    /// </summary>
    public const ushort C_MAX_GOALS = 3;

    /// <summary>
    /// How deep can one node tree go at most.
    /// </summary>
    public const byte C_MAX_DEPTH = byte.MaxValue;

    /// <summary>
    /// The node id
    /// </summary>
    public short Id { get; set; }

    /// <summary>
    /// The node that comes before this one, if -1 then it's root.
    /// </summary>
    public short ParentId { get; set; }

    /// <summary>
    /// The goals part of this node.
    /// </summary>
    public List<Goal> Goals { get; set; } = [];

    /// <summary>
    /// The children nodes.
    /// </summary>
    public List<Node> Children { get; set; } = [];
}
