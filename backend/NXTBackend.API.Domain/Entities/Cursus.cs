// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Domain.Joins;

// ============================================================================

namespace NXTBackend.API.Domain.Entities;

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_cursus")]
[Index(nameof(Name))]
[Index(nameof(Slug))]
public class Cursus : BaseEntity
{
    public Cursus()
    {
        Name = string.Empty;
        Description = string.Empty;
        Markdown = string.Empty;
        Slug = string.Empty;
        Deprecated = false;
        Public = false;
        Enabled = false;
        
        CreatorId = Guid.Empty;
        Creator = null!;
    }

    /// <summary>
    /// The name of the Cursus.
    /// </summary>
    [Column("name"), Required]
    public string Name { get; set; }

    /// <summary>
    /// A short description of the Cursus.
    /// </summary>
    [Column("description"), StringLength(256), Required]
    public string Description { get; set; }

    /// <summary>
    /// The markdown description of the Cursus.
    /// </summary>
    [Column("markdown")]
    public string Markdown { get; set; }

    /// <summary>
    /// The URL friendly slug for the Cursus.
    /// </summary>
    [Column("slug"), Required]
    public string Slug { get; set; }

    /// <summary>
    /// The Cursus is public / visible to anyone.
    /// </summary>
    [Column("public"), Required]
    public bool Public { get; set; }

    /// <summary>
    /// The Cursus allows for subscribers to create new Cursus based on this Cursus.
    /// </summary>
    [Column("enabled"), Required]
    public bool Enabled { get; set; }

    /// <summary>
    /// The Cursus allows for subscribers to create new Cursus based on this Cursus.
    /// </summary>
    [Column("deprecated"), Required]
    public bool Deprecated { get; set; }

    /// <summary>
    /// The kind of cursus.
    /// </summary>
    [Column("kind"), EnumDataType(typeof(CursusKind))]
    public CursusKind Kind { get; set; }

    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public virtual User Creator { get; set; }

    /// <summary>
    /// The track / path of the Cursus stored in the .graph format.
    /// </summary>
    [Column("track", TypeName = "jsonb")]
    public string? Track { get; set; }

    /// <summary>
    /// The different cursus sessions that exist for this cursus.
    /// </summary>
    public virtual ICollection<UserCursus> UserCursi { get; set; }
    
    public virtual ICollection<CursusCollaborator> Collaborators { get; set; }
}
