// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class FeatureDO : BaseObjectDO<Feature>
{
    public FeatureDO(Feature feature) : base(feature)
    {
        Name = feature.Name;
        Markdown = feature.Markdown;
    }

    public string Name { get; set; }

    public string Markdown { get; set; }

    public static implicit operator FeatureDO?(Feature? entity) => entity is null ? null : new(entity);
}
