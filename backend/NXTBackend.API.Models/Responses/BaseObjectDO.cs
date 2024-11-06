// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;

namespace NXTBackend.API.Models.Responses;

// ============================================================================

/// <summary>
/// Base Data Object used to convert models into data object that can be
/// transfered to other clients.
/// </summary>
/// <remarks>
/// It is important to note that some object needs to eagerload and others lazy
/// </remarks>
/// <typeparam name="T">Any object that is a Base Entity</typeparam>
/// <param name="entity"></param>
public abstract class BaseObjectDO<T>(T entity) where T : BaseEntity
{
    /// <summary>
    /// The unique identifier for this object.
    /// </summary>
    public Guid Id { get; set; } = entity.Id;

    /// <summary>
    /// Was created at.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = entity.CreatedAt;

    /// <summary>
    /// Updated at.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; } = entity.UpdatedAt;
}
