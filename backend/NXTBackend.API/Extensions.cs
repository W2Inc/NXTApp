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
    private const string C_CONTEXT_USER = "__user";

    /// <summary>
    /// Get the current authenticated user for the given context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static User? GetUser(this HttpContext context)
    {
        return context.Items[C_CONTEXT_USER] as User;
    }

    /// <summary>
    /// Set authenticated the current user for the given context.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    public static void SetUser(this HttpContext context, User? user)
    {
        context.Items[C_CONTEXT_USER] = user;
    }
}
