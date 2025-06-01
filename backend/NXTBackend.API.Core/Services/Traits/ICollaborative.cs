// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Traits;

// ============================================================================

/// <summary>
/// Interface for entities that support collaboration functionality,
/// allowing other users to access and modify the entity.
/// </summary>
public interface ICollaborative<T> where T : class
{
    /// <summary>
    /// Finds an entity by ID where the specified user is either a collaborator or the owner.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity to find.</param>
    /// <param name="userId">The unique identifier of the user who might be the owner or collaborator.</param>
    /// <returns>A tuple containing two nullable values:
    /// - Item1 (T?): The entity if found, otherwise null.
    /// - Item2 (U?): The collaboration details if the user is a collaborator, otherwise null.</returns>
    /// <remarks>
    /// If the user is either an owner or a collaborator, user is not null.
    /// </remarks>
    Task<(T?, User?)> IsCollaborator(Guid entityId, Guid userId);

    /// <summary>
    /// Adds a user as collaborator to this entity
    /// </summary>
    /// <param name="userId">The ID of the user to add</param>
    /// <returns>True if successfully added, false if already a collaborator</returns>
    Task<bool> AddCollaborator(Guid entityId, Guid userId);

    /// <summary>
    /// Removes a user from collaborators
    /// </summary>
    /// <param name="userId">The ID of the user to remove</param>
    /// <returns>True if successfully removed, false if wasn't a collaborator</returns>
    Task<bool> RemoveCollaborator(Guid entityId, Guid userId);
}
