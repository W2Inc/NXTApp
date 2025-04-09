// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

// ============================================================================

namespace NXTBackend.API.Domain.Entities;


/// <summary>
/// An event
/// </summary>
[Table("tbl_event")]
public class Event : BaseEntity
{
    public Event()
    {
        Title = string.Empty;
        Subtitle = string.Empty;
        Location = string.Empty;
        StartsAt = DateTimeOffset.UtcNow;
        EndsAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// The title of the event
    /// </summary>
    [Column("title")]
    public string Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("subtitle")]
    public string Subtitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column("subtitle")]
    public string Location { get; set; }

    /// <summary>
    /// When the feature was created.
    /// </summary>
    [Column("starts_at")]
    public DateTimeOffset StartsAt { get; set; }

    /// <summary>
    /// Updated at.
    /// </summary>
    [Column("ends_at")]
    public DateTimeOffset EndsAt { get; set; }
}
