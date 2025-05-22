// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Responses.Objects;

public class NotificationDO : BaseObjectDO<Notification>
{
    public NotificationDO(Notification notification) : base(notification)
    {
        // Message = notification.Message;
        // Kind = notification.Kind;
    }

    /// <summary>
    /// The notification message
    /// </summary>
    // [Required]
    // public string Message { get; set; }

    /// <summary>
    /// The type of notification
    /// </summary>
    // [Required]
    // public NotificationKind Kind { get; set; }

    /// <summary>
    /// Optional reference to a resource this notification is about
    /// </summary>
    public Guid? ResourceId { get; set; }

    public static implicit operator NotificationDO?(Notification? entity) => entity is null ? null : new NotificationDO(entity);
}
