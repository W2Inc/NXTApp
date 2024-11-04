// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class ProjectDO(Project project) : BaseDO<Project>(project)
{
    public string Name { get; set; } = project.Name;

    public string Markdown { get; set; } = project.Markdown;

    public string Slug { get; set; } = project.Slug;

    public string? ThumbnailUrl { get; set; } = project.ThumbnailUrl;

    public bool Public { get; set; } = project.Public;

    public bool Enabled { get; set; } = project.Enabled;

    public int MaxMembers { get; set; } = project.MaxMembers;

    public Guid GitInfoId { get; set; } = project.GitInfoId;

    public virtual Git GitInfo { get; set; } = project.GitInfo;

    public Guid CreatorId { get; set; } = project.CreatorId;

    public virtual UserDO? Creator { get; set; } = project.Creator;

    public string[] Tags { get; set; } = project.Tags;

    public static implicit operator ProjectDO?(Project? entity) => entity is null ? null : new ProjectDO(entity);
}
