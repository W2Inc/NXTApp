// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Traits;

// ============================================================================

/// <summary>
/// Interface for entities that support collaboration functionality,
/// allowing other users to access and modify the entity.
/// </summary>
public interface ICollaborative<T>
{
    /// <summary>
    /// Finds an entity by ID where the specified user is either a collaborator or the owner.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity to find.</param>
    /// <param name="userId">The unique identifier of the user who might be the owner or collaborator.</param>
    /// <returns>A tuple containing two nullable values:
    /// - Item1 (T?): The entity if found, otherwise null.
    /// - Item2 (U?): The collaboration details if the user is a collaborator, otherwise null.</returns>
    /// <remarks>If the user is the owner, the first item will contain the entity and the second will be null.
    /// If the user is a collaborator, both items will be populated.</remarks>
    Task<(T?, User?)> FindAsCollaboratorOrOwner(Guid entityId, Guid userId);

    /// <summary>
    /// Adds a user as collaborator to this entity
    /// </summary>
    /// <param name="userId">The ID of the user to add</param>
    /// <returns>True if successfully added, false if already a collaborator</returns>
    Task<bool> AddCollaborator(Guid userId);

    /// <summary>
    /// Removes a user from collaborators
    /// </summary>
    /// <param name="userId">The ID of the user to remove</param>
    /// <returns>True if successfully removed, false if wasn't a collaborator</returns>
    Task<bool> RemoveCollaborator(Guid userId);
}
