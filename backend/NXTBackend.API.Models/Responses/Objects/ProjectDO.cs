// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
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

    [Required]
    public string Name { get; set; }

    [Required]
    public string? Markdown { get; set; }

    [Required]
    public string Slug { get; set; }

    [Required]
    public string? ThumbnailUrl { get; set; }

    [Required]
    public bool Public { get; set; }

    [Required]
    public bool Enabled { get; set; }

    [Required]
    public int MaxMembers { get; set; }

    [Required]
    public virtual GitDO? GitInfo { get; set; }

    [Required]
    public virtual MinimalUserDTO? Creator { get; set; }

    [Required]
    public string[] Tags { get; set; }
}
