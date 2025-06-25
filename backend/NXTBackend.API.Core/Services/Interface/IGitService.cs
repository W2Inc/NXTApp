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
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Git entity.
/// </summary>
public interface IGitService : IDomainService<Git>, ICollaborative<Git>
{
	/// <summary>
	/// Creates a new repository.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="owner"></param>
	/// <param name="description"></param>
	/// <param name="defaultBranch"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public Task<Git> CreateRepository(string name, string owner, string? description = null, string defaultBranch = "main", OwnerKind type = OwnerKind.User);

	/// <summary>
	/// Archives a repository.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task<Git> ArchiveRepository(Guid id);

	/// <summary>
	/// Upserts a file in the repository.
	/// If the file does not exist, it will be created.
	/// If content is null, the file will be deleted.
	/// </summary>
	/// <param name="id">The Git id</param>
	/// <param name="path">The path for the file</param>
	/// <param name="content">Base64 encoded content of the file</param>
	/// <param name="message">Optional commit message</param>
	/// <param name="branch">The target branch</param>
	/// <returns></returns>
	public Task<Git> UpsertFile(Guid id, string path, string? content, string message = "Update", string branch = "main");

	/// <summary>
	///
	/// </summary>
	/// <param name="id"></param>
	/// <param name="path"></param>
	/// <param name="branch"></param>
	/// <returns></returns>
	public Task<IEnumerable<GitVfsNodeDO>> GetFiles(Guid id, string path, string branch = "main");

    /// <summary>
    /// Get the current latest hash of the remote.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IEnumerable<GitRefDO>> GetRefs(Guid id);
}
