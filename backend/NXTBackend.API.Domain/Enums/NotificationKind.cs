// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

// ============================================================================

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The state of a member invite.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationKind
{
    /// <summary>
    /// Simple notification, basically just a message.
    /// </summary>
    [JsonPropertyName(nameof(Default))]
    Default,

    /// <summary>
    /// Simple notification, basically just a message.
    /// </summary>
    [JsonPropertyName(nameof(Invite))]
    Invite,

    /// <summary>
    /// System wide notification visible as broadcasts.
    /// </summary>
    [JsonPropertyName(nameof(System))]
    System,
}
