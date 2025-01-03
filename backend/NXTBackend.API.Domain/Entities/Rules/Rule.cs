// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Domain.Entities.Rules;

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_rules")]
public class Rule : BaseEntity
{
    public Rule()
    {
        Name = string.Empty;
        QueryTemplate = string.Empty;
        Enabled = false;
    }

    /// <summary>
    /// The name of the Cursus.
    /// </summary>
    [Column("name"), Required]
    public string Name { get; set; }

    /// <summary>
    /// The Cursus allows for subscribers to create new Cursus based on this Cursus.
    /// </summary>
    [Column("enabled"), Required]
    public bool Enabled { get; set; }

    /// <summary>
    /// A WHERE query clause to be attached when evaluating if a user can do
    ///
    /// e.g: WHERE task_name = $1::text AND completed = $2::boolean;
    /// e.g: WHERE start_date >= $1::timestamp AND end_date <= $2::timestamp;
    /// </summary>
    [Column("query_template"), Required]
    public string QueryTemplate { get; set; }
}
