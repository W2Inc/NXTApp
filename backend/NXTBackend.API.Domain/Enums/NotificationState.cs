// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

// ============================================================================

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The type of action the user undertook for a given notification
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationState
{
    /// <summary>
    /// The user has taken no action on this notification thus far
    /// </summary>
    [JsonPropertyName(nameof(None))]
    None,

    /// <summary>
    /// The user marked this notification as read
    /// </summary>
    [JsonPropertyName(nameof(Read))]
    Read,
}
