using System;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Shared;

/// <summary>
/// Data transfer object representing serialized notification data for processing.
/// </summary>
public class NotificationDataDO
{
    /// <summary>
    /// The plain text content of the notification.
    /// Null if the notification cannot be transformed to text.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// The HTML content of the notification.
    /// Null if the notification cannot be transformed to HTML.
    /// </summary>
    public string? Html { get; init; }

    /// <summary>
    /// The subject of the notification for email purposes.
    /// </summary>
    public string? Subject { get; init; }

    /// <summary>
    /// The recipient's address (email, phone number, etc.).
    /// </summary>
    public string? RecipientAddress { get; init; }
}
