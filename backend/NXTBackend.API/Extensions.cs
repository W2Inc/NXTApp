// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Security.Claims;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API;

public static class Extensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (id is null)
            return null;

        return Guid.TryParse(id.Value, out var guid) ? guid : null;
    }

    public async static Task<User?> GetUser(this HttpContext context, IUserService service)
    {
        var id = context.GetUserId();
        return id is null ? null : await service.FindByIdAsync(id.Value);
    }

    public static string? GetClaimValue(this HttpContext context, string claim) => context.User.Claims.FirstOrDefault(c => c.Type == claim)?.Value;


    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="service"></param>
    /// <returns></returns>
    public async static Task<User?> EnsureUser(this HttpContext context, IUserService service)
    {

        var id = context.GetUserId();
        if (id is null)
            return null;
        var user = await service.FindByIdAsync(id.Value);
        if (user is null)
            return null;
        if (!Guid.TryParse(context.GetClaimValue(ClaimTypes.NameIdentifier), out var userID))
        {
            return null;
        }

        var login = context.GetClaimValue(ClaimTypes.GivenName);
        if (login is null)
            return null;

        await service.CreateAsync(new()
        {
            Id = userID,
            Login = login,
            DisplayName = context.GetClaimValue(ClaimTypes.GivenName),
        });


        return user;
    }

}
