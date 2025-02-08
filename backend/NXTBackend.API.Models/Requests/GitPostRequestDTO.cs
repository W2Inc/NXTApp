// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Domain.Enums;
using System.ComponentModel.DataAnnotations;

// ============================================================================

namespace NXTBackend.API.Models.Requests;

/// <summary>
/// Request for setting up a git repository
/// </summary>
public class GitInfoRequestDto : BaseRequestDTO
{
    // TODO: Should be a more stricter type
    [Required]
    public string GitNamespace { get; set; }

    [Required]
    public string GitName { get; set; }

    [Required, Url]
    public string GitUrl { get; set; }

    [Required]
    public string GitBranch { get; set; }

    [Required]
    public GitProviderKind SourceKind { get; set; }
}
