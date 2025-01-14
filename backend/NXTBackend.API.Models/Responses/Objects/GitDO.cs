// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;

// ============================================================================

namespace NXTBackend.API.Models.Responses.Objects;

public class GitDO : BaseObjectDO<Git>
{
    public GitDO(Git git) : base(git)
    {
        GitUrl = git.GitUrl;
        GitBranch = git.GitBranch;
        GitCommit = git.GitCommit;
    }

    [Required]
    public string GitUrl { get; set; }

    [Required]
    public string GitBranch { get; set; }

    [Required]
    public string? GitCommit { get; set; }

    public static implicit operator GitDO?(Git? entity) => entity is null ? null : new(entity);
}
