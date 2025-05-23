﻿// ============================================================================
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

namespace NXTBackend.API.Domain.Entities;

/*
model Cursus {
    id         String   @id @default(dbgenerated("gen_random_uuid()")) @db.Uuid
    created_at DateTime @default(now())
    updated_at DateTime @default(now()) @updatedAt

    name        String @unique
    description String @db.VarChar(256)
    markdown    String

    slug String @unique

    // If false then it's not visible to other users, except owner
    public     Boolean @default(false)
    // Allow for new user cursus to be created based on this cursus
    enabled    Boolean @default(false)
    // Everything is abandoned and no longer maintained
    deprecated Boolean @default(false)

    // If the cursus is readonly, meaning it's a pre-defined path then it can't be modified
    // and as such the path_id is required! The goal's can't be changed either.
    //
    // If the cursus is not readonly, then the path is yet to be fully defined.
    // In essence the user constructs this path themselves over time.
    readonly Boolean @default(false)

    owner_id String @db.Uuid
    owner    User   @relation("CursusCreator", fields: [owner_id], references: [id])

    // path_id String?       @unique @db.Uuid
    // path    CursusVertex? @relation("CursusPath", fields: [path_id], references: [id])

    vertices CursusVertex[] @relation("CursusVertices")

    user_cursi UserCursus[]

    @@map("cursus")
}
*/

// ============================================================================

/// <summary>
/// A feature is a experimental feature that is being developed.
/// </summary>
[Table("tbl_cursus")]
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
    public virtual User Creator { get; set; } = null!;

    /// <summary>
    /// The track / path of the Cursus stored in the .graph format.
    /// </summary>
    [Column("track", TypeName = "jsonb")]
    public string? Track { get; set; }

    /// <summary>
    /// The different cursus sessions that exist for this cursus.
    /// </summary>
    public virtual ICollection<UserCursus> UserCursi { get; set; }

    public virtual ICollection<User> Collaborators { get; set; }
}
