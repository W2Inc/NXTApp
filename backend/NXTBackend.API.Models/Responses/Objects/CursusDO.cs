// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class CursusDO : BaseDO<Cursus>
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

    public string Name { get; set; }

    public string Description { get; set; }

    public string Markdown { get; set; }

    public string Slug { get; set; }

    public bool Public { get; set; }

    public bool Enabled { get; set; }

    public CursusKind Kind { get; set; }

    public UserDO? Creator { get; set; }

    public static implicit operator CursusDO?(Cursus? entity) => entity is null ? null : new(entity);
}
