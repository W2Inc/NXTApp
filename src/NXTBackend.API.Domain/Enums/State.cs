// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different states that an entity can have.
/// 
/// For example the state of a project
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State
{
    /// <summary>
    /// The entity is inactive.
    /// </summary>
    Inactive,

    /// <summary>
    /// The entity is active.
    /// </summary>
    Active,

    /// <summary>
    /// The entity is awaiting something.
    /// </summary>
    Awaiting,

    /// <summary>
    /// The entity is on going.
    /// </summary>
    OnGoing,

    /// <summary>
    /// The entity is completed.
    /// </summary>
    Completed
}
