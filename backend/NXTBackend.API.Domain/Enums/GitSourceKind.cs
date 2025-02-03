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
public enum GitSourceKind
{
    /// <summary>
    /// External source control other than Github. E.g: (Gitea, Gitlab, Custom Home server, ...)
    /// </summary>
    [JsonPropertyName(nameof(Thirdparty))]
    Thirdparty,

    /// <summary>
    /// Uses the backend configured git source control, the ideal *boomer* option as most people just
    /// want to "upload a file" but we want to provide source control towards whatever they provide.
    /// </summary>
    [JsonPropertyName(nameof(Managed))]
    Managed,

    /// <summary>
    /// Uses github integration to manage the git source control, this is essentially the same as
    /// third party in essence except that because github is the more popular variant we *may* want
    /// to offer some more features that they provide.
    /// </summary>
    [JsonPropertyName(nameof(Github))]
    Github,
}
