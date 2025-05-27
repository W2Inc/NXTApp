using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace NXTBackend.API.Domain.Common;

// NOTE: https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations

public interface ISortableEntity<T> where T : BaseEntity
{
    static abstract Expression<Func<T, object>>? GetKeySelectorFor(string sortKey);
    static abstract string[] GetAvailableSortKeys();
}

/// <summary>
/// Base entity for all entities in the system.
/// </summary>
public abstract class BaseEntity
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity
    [Column("id", Order = 0), Key]
    public Guid Id { get; set; } = Guid.CreateVersion7();

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
    
    /// <summary>
    /// Function to retrieve the order by property.
    ///
    /// If the property is order-able it is returned, else null.
    /// </summary>
    /// <param name="key">The supposed property name (</param>
    /// <returns></returns>
    public static Expression<Func<BaseEntity, object>>? GetOrderByKey(string? key)
    {
        return key?.ToLowerInvariant() switch
        {
            // BaseEntity properties
            "id" => entity => entity.Id,
            "createdat" => entity => entity.CreatedAt,
            "updatedat" => entity => entity.UpdatedAt,
            _ => null
        };
    }
}
