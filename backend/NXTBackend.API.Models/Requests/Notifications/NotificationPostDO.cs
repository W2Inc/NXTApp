// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Requests;

public class NotificationPostDTO
{
    /// <summary>
    /// The message content of the notification
    /// </summary>
    [Required, MinLength(1), MaxLength(256)]
    public string Message { get; set; }

    /// <summary>
    /// The type of notification
    /// </summary>
    // [Required]
    // public NotificationKind Kind { get; set; }

    /// <summary>
    /// The ID of the user to send the notification to
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Optional reference to a resource this notification is about
    /// </summary>
    public Guid? ResourceId { get; set; }
}
