﻿// ============================================================================
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
public enum MemberInviteState
{
    /// <summary>
    /// The user has been invited to join the project.
    /// </summary>
    [JsonPropertyName(nameof(Pending))]
    Pending,

    /// <summary>
    /// The user has accepted the invite to join the project.
    /// </summary>
    [JsonPropertyName(nameof(Accepted))]
    Accepted,
}
