// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Models.Validators;

// ============================================================================

namespace NXTBackend.API.Models.Requests.User;

//pub role: Option<UserRole>,
//    pub display_name: Option<String>,
//    pub avatar_url: Option<String>,
//    pub access_token: Option<String>,

/// <summary>
/// Post request for creating a new cursus
/// </summary>
public class UserPatchRequestDto : BaseRequestDto
{
    [UserDisplayNameValidation, StringLength(64, MinimumLength = 1)]
    public string? DisplayName { get; set; }

    [Url]
    public string? AvatarUrl { get; set; }
}