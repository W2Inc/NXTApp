﻿// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different kinds of reviews that exist for a project.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReviewKind
{
    /// <summary>
    /// A reflection on the project by the author themselves.
    /// </summary>
    [JsonPropertyName(nameof(Self))]
    Self,

    /// <summary>
    /// A review by a peer of the author.
    /// </summary>
    [JsonPropertyName(nameof(Peer))]
    Peer,

    /// <summary>
    /// A independent of time or order review (e.g. a review by someone from another country).
    /// </summary>
    [JsonPropertyName(nameof(Async))]
    Async,

    /// <summary>
    /// A review that is automatically generated by a set of unit tests.
    /// </summary>
    [JsonPropertyName(nameof(Auto))]
    Auto
}
