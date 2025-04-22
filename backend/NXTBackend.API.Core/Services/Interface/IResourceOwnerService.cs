// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IResourceOwnerService
{
    /// <summary>
    /// Find either a user or organization by ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The resource owner found by that ID or null if not found.</returns>
    public Task<ResourceOwner?> FindByIdAsync(Guid id);
}
