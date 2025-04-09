// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations.Schema;
using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Domain.Entities;

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_feature")]
public class Feature : BaseEntity
{
    public Feature()
    {
        IsPublic = false;
        Name = string.Empty;
        Markdown = string.Empty;
    }

    /// <summary>
    /// The name of the feature.
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// The markdown content of the feature.
    /// </summary>
    [Column("markdown")]
    public string Markdown { get; set; }

    /// <summary>
    /// Is this feature public?
    /// </summary>
    [Column("is_public")]
    public bool IsPublic { get; set; }
}
