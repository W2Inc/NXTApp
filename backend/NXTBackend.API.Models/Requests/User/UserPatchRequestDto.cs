// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Models.Validators;

// ============================================================================

namespace NXTBackend.API.Models.Requests.User;

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class UserPatchRequestDTO : BaseRequestDTO
{
    [UserDisplayNameValidation, StringLength(64, MinimumLength = 1)]
    public string? DisplayName { get; set; }

    [Url]
    public string? AvatarUrl { get; set; }
}
