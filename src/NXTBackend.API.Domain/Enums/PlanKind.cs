// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different kinds of cursi that exist.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PlanKind
{
    /// <summary>
    /// A free plan is a plan that is free to use.
    /// </summary>
    [JsonPropertyName(nameof(Free))]
    Free,

    /// <summary>
    /// A paid plan is a plan that requires a payment to use.
    /// </summary>
    [JsonPropertyName(nameof(Paid))]
    Paid,
}
