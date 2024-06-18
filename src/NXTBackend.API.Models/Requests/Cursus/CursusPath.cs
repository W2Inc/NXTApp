// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests.Cursus;

/// <summary>
/// The path of the cursus, meaning a tree with nodes.
/// In <see cref="CursusService"/> we ensure that the tree does not
/// exceed a certain depth. e.g. 4 levels deep.
/// </summary>
public class CursusPath
{
    /// <summary>
    /// The learning goals present in this "node",
    /// if multiple are present it's an indication that there is
    /// a choice between 1 and N.
    /// </summary>
    [MaxLength(4)]
    public ICollection<Guid> Goals { get; set; } = [];

    /// <summary>
    /// The next "nodes" in the tree
    /// </summary>
    [MaxLength(6)]
    public ICollection<CursusPath> Next { get; set; } = [];
}