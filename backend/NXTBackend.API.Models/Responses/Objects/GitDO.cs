// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class GitDO : BaseObjectDO<Git>
{
    public GitDO(Git git) : base(git)
    {
        Name = git.Name;
        Namespace = git.Namespace;
        Url = git.Url;
        Branch = git.Branch;
        ProviderType = git.ProviderType;
        OwnerType = git.OwnerType;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Namespace { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public string Branch { get; set; }

    [Required]
    public GitProviderKind ProviderType { get; set; }

    [Required]
    public OwnerKind OwnerType { get; set; }

    public static implicit operator GitDO?(Git? entity) => entity is null ? null : new(entity);
}
