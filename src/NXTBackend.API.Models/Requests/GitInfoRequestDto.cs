// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// Request for setting up a git repository
/// </summary>
public class GitInfoRequestDto : BaseRequestDto
{
    /// <summary>
    /// The URL to the git repository
    /// </summary>
    /// <example>
    /// https://github.com/W2Wizard/NXTBackend
    /// </example>
    [Required, Url]
    public string GitUrl { get; set; } = string.Empty;

    /// <summary>
    /// If null, will use the master branch
    /// </summary>
    public string? GitBranch { get; set; }

    /// <summary>
    /// If null, will use the latest commit
    /// </summary>
    public string? GitCommit { get; set; }
}