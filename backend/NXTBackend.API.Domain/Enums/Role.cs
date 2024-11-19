// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different roles that a user can have.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    /// <summary>
    /// Starting role for all users.
    /// </summary>
    [JsonPropertyName(nameof(Default))]
    Default,

    /// <summary>
    /// Role for users that have been granted access to the admin panel.
    /// </summary>
    [JsonPropertyName(nameof(Admin))]
    Admin,

    /// <summary>
    /// Developer role
    /// </summary>
    [JsonPropertyName(nameof(Developer))]
    Developer,
}
