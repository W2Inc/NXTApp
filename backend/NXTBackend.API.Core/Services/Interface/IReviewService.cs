// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Core.Services.Interface;
/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IReviewService : IDomainService<Review>
{
    /// <summary>
    /// Find the entity by its ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The entity found by that ID or null if not found.</returns>
    public Task<Review?> FindByIdAsync(Guid id);
}
