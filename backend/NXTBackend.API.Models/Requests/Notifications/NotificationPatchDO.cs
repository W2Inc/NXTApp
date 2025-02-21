// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Requests;

public class NotificationPatchDTO
{
    /// <summary>
    /// The message content of the notification
    /// </summary>
    [MinLength(1), MaxLength(500)]
    public string? Message { get; set; }

    /// <summary>
    /// The type of notification
    /// </summary>
    public NotificationKind? Kind { get; set; }

    /// <summary>
    /// Optional reference to a resource this notification is about
    /// </summary>
    public Guid? ResourceId { get; set; }
}
