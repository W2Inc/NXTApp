// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different kinds of feeds that exist.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberNameAttribute)), Flags]
public enum SearchKind
{

}
