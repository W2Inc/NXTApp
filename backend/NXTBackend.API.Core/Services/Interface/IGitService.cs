// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Responses.Gitea;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Git entity.
/// </summary>
public interface IGitService : IDomainService<Git>
{
    /// <summary>
    /// Creates a remote Git repository.
    /// </summary>
    /// <param name="repositoryName">Name of the repository to create</param>
    /// <param name="description">Optional description for the repository</param>
    /// <returns>The created Git repository details</returns>
    Task<Git> CreateRemoteRepository(string repositoryName, string? description = null);

    /// <summary>
    /// Gets the raw content of a file from a repository.
    /// </summary>
    /// <param name="owner">Owner of the repository</param>
    /// <param name="repo">Name of the repository</param>
    /// <param name="path">Path to the file</param>
    /// <param name="branch">Branch name (defaults to main)</param>
    /// <returns>The raw content of the file as a string</returns>
    Task<string> GetRawFileContent(
        string name,
        string path,
        string branch = "main"
    );

    /// <summary>
    /// Archives a repository.
    /// </summary>
    /// <param name="owner">Owner of the repository</param>
    /// <param name="repo">Name of the repository</param>
    Task ArchiveRepository(string owner, string repo);

    /// <summary>
    /// Creates or updates a file in a repository.
    /// </summary>
    /// <param name="owner">Owner of the repository</param>
    /// <param name="repo">Name of the repository</param>
    /// <param name="path">Path to the file</param>
    /// <param name="content">Content of the file</param>
    /// <param name="message">Commit message</param>
    /// <param name="branch">Branch name (defaults to main)</param>
    /// <returns>Response containing the file and commit information</returns>
    Task<FileContentResponse> UpsertFile(
        string owner,
        string repo,
        string path,
        string content,
        string message,
        string branch = "main"
    );
}
