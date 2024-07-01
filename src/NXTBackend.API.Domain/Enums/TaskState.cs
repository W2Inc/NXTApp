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
public enum TaskState
{
    /// <summary>
    /// The task is inactive and won't continue.
    /// </summary>
    Inactive,

    /// <summary>
    /// The task is active and will continue.
    /// </summary>
    Active,

    /// <summary>
    /// The task is awaiting something.
    /// </summary>
    Awaiting,

    /// <summary>
    /// The entity is completed.
    /// </summary>
    Completed
}
