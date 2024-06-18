// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReviewState
{
    Pending,
    InProgress,
    Finished
}
