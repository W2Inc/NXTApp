// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Requests.ExternalGit;
using NXTBackend.API.Models.Responses.Gitea;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Git entity.
/// </summary>
public interface IGitService : IDomainService<Git>, ICollaborative<Git>
{
    /// <summary>
    /// Creates a new remote repository
    /// </summary>
    /// <param name="DTO">The Data Transfer Object describing the Repos creation parameters.</param>
    /// <returns>The Git information.</returns>
    public Task<Git> CreateRepository(GitRepoPostRequestDTO DTO, OwnerKind OwnerType);

    /// <summary>
    /// Hard delete a repository.
    ///
    /// WARNING: Should not be used to deprecate / archive repositories.
    /// This function is intended to clean up mishaps such as:
    ///     - Creating a repo + project but project creation fails.
    /// </summary>
    /// <returns></returns>
    public Task DeleteRepository(Guid id);

    /// <summary>
    /// Updates certain Repository settings
    /// </summary>
    /// <param name="GitNamespace">The namespace to update</param>
    /// <param name="DTO">The Data Transfer Object describing the Repos update parameters.</param>
    /// <returns></returns>
    public Task<Git> UpdateRepository(Guid id, GitRepoPatchRequestDTO DTO);

    /// <summary>
    /// Get the file contents of a file in a given namespace and path.
    /// </summary>
    /// <param name="GitNamespace"></param>
    /// <param name="Path"></param>
    /// <param name="Branch"></param>
    /// <returns></returns>
    public Task<string> GetFile(Guid id, string Path, string Branch = "main");

    /// <summary>
    /// Sets or updates a file in a Git repository with the specified content.
    /// </summary>
    /// <param name="GitNamespace">The Git namespace or repository path where the file should be set.</param>
    /// <param name="Path">The path to the file within the repository.</param>
    /// <param name="Content">The content to be written to the file.</param>
    /// <param name="CommitMessage">The message describing the changes in the commit.</param>
    /// <param name="Branch">The branch where the file should be updated. Defaults to "main".</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetFile(Guid id, string Path, string Content, string CommitMessage, string Branch = "main");

    /// <summary>
    /// Retrieves the hash for the current git info from the remote.
    /// </summary>
    /// <param name="id">The git info to get the hash for</param>
    /// <param name="Branch">From the specific branch</param>
    /// <returns>The SHA1 hash of the repository for a given branch</returns>
    public Task<string> GetLatestHash(Guid id, string Branch = "main");
}
