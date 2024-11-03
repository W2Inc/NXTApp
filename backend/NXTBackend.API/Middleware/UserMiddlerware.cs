// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Security.Claims;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Users;

// ============================================================================

namespace NXTBackend.API.Middleware;

/// <summary>
///
/// </summary>
/// <param name="next">The next request</param>
/// <param name="log">Logging service</param>
public class UserMiddlerware(RequestDelegate next, ILogger<UserMiddlerware> log)
{
    public async Task InvokeAsync(HttpContext context, IUserService service)
    {
        context.SetUser(null);

        var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (id is null || Guid.TryParse(id.Value, out var guid))
        {
            log.LogDebug("No authenticated user");
            await next(context);
            return;
        }

        // If the routes requires a user to be authenticated. We can already
        // fetch it and add it as an item to the context for later.
        var endpoint = context.GetEndpoint();
        var authorizeAttributes = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();
        if (authorizeAttributes is not null)
        {
            // The endpoint does require a authenticated user...
            var user = await service.FindByIdAsync(guid);
            log.LogDebug("Authenticated user: {@id}", user?.Id ?? Guid.Empty);
            context.SetUser(user);
        }

        await next(context);
        return;
    }
}
