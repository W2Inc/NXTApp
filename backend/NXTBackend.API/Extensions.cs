// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Security.Claims;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API;

public static class Extensions
{
    /// <summary>
    /// Get the current user sid.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static Guid GetSID(this ClaimsPrincipal principal)
    {
        string? claim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;
    }

    /// <summary>
    /// Get the current user login name.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static Claim? GetPreferredUsername(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("preferred_username");
    }

    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        var identity = principal.Identities.First();
        return identity != null && identity.IsAuthenticated;
    }
}
