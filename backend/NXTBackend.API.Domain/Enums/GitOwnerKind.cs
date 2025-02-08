// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different kinds of cursi that exist.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GitOwnerKind
{
    /// <summary>
    /// External source control other than Github. E.g: (Gitea, Gitlab, Custom Home server, ...)
    /// </summary>
    [JsonPropertyName(nameof(User))]
    User,

    /// <summary>
    /// Uses the backend configured git source control, the ideal *boomer* option as most people just
    /// want to "upload a file" but we want to provide source control towards whatever they provide.
    /// </summary>
    [JsonPropertyName(nameof(Organization))]
    Organization,
}
