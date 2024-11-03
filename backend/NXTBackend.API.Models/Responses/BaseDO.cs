// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;

// ============================================================================

/// <summary>
/// Base Data Object used to convert models into Data object that can be
/// transfered to other clients.
/// </summary>
/// <typeparam name="T">Any object that is a Base Entity</typeparam>
/// <param name="entity"></param>
public abstract class BaseDO<T>(T entity) where T : BaseEntity
{
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
