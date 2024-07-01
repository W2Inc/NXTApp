using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXTBackend.API.Domain.Common;

// NOTE: https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations

/// <summary>
/// Base entity for all entities in the system.
/// </summary>
public abstract class BaseEntity
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity
    [Column("id"), Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// When the feature was created.
    /// </summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Updated at.
    /// </summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}