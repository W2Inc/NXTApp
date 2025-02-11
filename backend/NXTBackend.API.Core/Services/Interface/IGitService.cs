﻿// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Core.Services.Interface;
/// <summary>
/// Service for the Rubric entity.
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
}
