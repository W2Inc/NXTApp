// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The type of owner for a specific entity.
///
/// E.G: A na
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OwnerKind
{

}
