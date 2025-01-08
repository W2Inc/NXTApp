// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

/// <summary>
/// Data object representing a Cursus entity.
/// </summary>
/// <param name="cursus">The Cursus entity to initialize the data object with.</param>
public class CursusDO : BaseObjectDO<Cursus>
{
    public CursusDO(Cursus cursus) : base(cursus)
    {
        Name = cursus.Name;
        Description = cursus.Description;
        Markdown = cursus.Markdown;
        Slug = cursus.Slug;
        Public = cursus.Public;
        Enabled = cursus.Enabled;
        Kind = cursus.Kind;
        Creator = cursus.Creator;
    }

    public static implicit operator CursusDO?(Cursus? entity) => entity is null ? null : new(entity);

    /// <summary>
    /// The name of the cursus.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// The description of the cursus.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// The markdown content of the cursus.
    /// </summary>
    [Required]
    public string Markdown { get; set; }

    /// <summary>
    /// The slug (URL-friendly identifier) of the cursus.
    /// </summary>
    [Required]
    public string Slug { get; set; }

    /// <summary>
    /// Whether the cursus is public.
    /// </summary>
    [Description("Whether the cursus is public")]
    [Required]
    public bool Public { get; set; }

    /// <summary>
    /// Whether the cursus is enabled.
    /// </summary>
    [Description("If true, the cursus is subscribable")]
    [Required]
    public bool Enabled { get; set; }

    /// <summary>
    /// The kind of the cursus.
    /// </summary>
    [Required]
    public CursusKind Kind { get; set; }

    /// <summary>
    /// The creator of the cursus.
    /// </summary>
    [Required]
    public MinimalUserDTO? Creator { get; set; }

}
