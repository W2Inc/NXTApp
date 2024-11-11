// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
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
    /// A new cursus has been added.
    /// </summary>
    [JsonPropertyName(nameof(NewCursus))]
    NewCursus,

    /// <summary>
    /// A new project has been added.
    /// </summary>
    [JsonPropertyName(nameof(NewProject))]
    NewProject,

    /// <summary>
    /// A new goal has been added.
    /// </summary>
    [JsonPropertyName(nameof(NewGoal))]
    NewGoal,

    /// <summary>
    /// A new campus event has been added.
    /// </summary>
    [JsonPropertyName(nameof(NewEvent))]
    NewEvent,
}
