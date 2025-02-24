// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Text.Json.Serialization;

namespace NXTBackend.API.Domain.Enums;

/// <summary>
/// The different kinds of feeds that exist.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter)), Flags]
public enum FeedKind
{
    /// <summary>
    /// A new user has joined the app
    /// </summary>
    [JsonPropertyName(nameof(NewUser))]
    NewUser = 1 << 0,

    /// <summary>
    /// Some user has completed a project, hoorah!
    /// </summary>
    [JsonPropertyName(nameof(CompletedProject))]
    CompletedProject = 1 << 1,

    /// <summary>
    /// Some user has completed a goal, hoorah!
    /// </summary>
    [JsonPropertyName(nameof(CompletedGoal))]
    CompletedGoal = 1 << 2,

    /// <summary>
    /// Some user has completed a cursus, hoorah!
    /// </summary>
    [JsonPropertyName(nameof(CompletedCursus))]
    CompletedCursus = 1 << 3,

    /// <summary>
    /// Current user received a review on their work
    /// </summary>
    [JsonPropertyName(nameof(ReceivedReview))]
    ReceivedReview = 1 << 4,

    /// <summary>
    /// A project has been deprecated
    /// </summary>
    [JsonPropertyName(nameof(ProjectDeprecated))]
    ProjectDeprecated = 1 << 5,

    /// <summary>
    /// A goal has been deprecated
    /// </summary>
    [JsonPropertyName(nameof(GoalDeprecated))]
    GoalDeprecated = 1 << 6,

    /// <summary>
    /// A cursus has been deprecated
    /// </summary>
    [JsonPropertyName(nameof(CursusDeprecated))]
    CursusDeprecated = 1 << 7,
}
