// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class ProjectDO : BaseObjectDO<Project>
{
    public ProjectDO(Project project) : base(project)
    {
        Name = project.Name;
        Markdown = project.Markdown;
        Slug = project.Slug;
        ThumbnailUrl = project.ThumbnailUrl;
        Public = project.Public;
        Enabled = project.Enabled;
        MaxMembers = project.MaxMembers;
        GitInfo = project.GitInfo;
        Creator = project.Creator;
        Tags = project.Tags;
    }

    public static implicit operator ProjectDO?(Project? entity) => entity is null ? null : new ProjectDO(entity);

    public string Name { get; set; }

    public string? Markdown { get; set; }

    public string Slug { get; set; }

    public string? ThumbnailUrl { get; set; }

    public bool Public { get; set; }

    public bool Enabled { get; set; }

    public int MaxMembers { get; set; }

    public virtual GitDO? GitInfo { get; set; }

    public virtual MinimalUserDTO? Creator { get; set; }

    public string[] Tags { get; set; }
}
