// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

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

    public string GitUrl { get; set; }

    public string GitBranch { get; set; }

    public string? GitCommit { get; set; }

    public static implicit operator GitDO?(Git? entity) => entity is null ? null : new(entity);
}
