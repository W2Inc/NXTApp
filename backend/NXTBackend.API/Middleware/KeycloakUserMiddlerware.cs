// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Middleware;

/// <summary>
/// Ensure that the user who has a valid claim, actually gets inserted
/// into the Database if they didn't exist before.
/// </summary>
/// <param name="next">The next request</param>
/// <param name="log">Logging service</param>
/// <param name="cache"></param>
public class KeycloakUserMiddlerware(RequestDelegate next, ILogger<KeycloakUserMiddlerware> log, IDistributedCache cache)
{
    public async Task InvokeAsync(HttpContext context, IUserService service)
    {
        var guid = context.User.GetSID();
        if (!context.User.IsAuthenticated() || guid == Guid.Empty)
        {
            await next(context);
            return;
        }

        string cacheKey = $"USER_{guid}";
        string? cachedUser = await cache.GetStringAsync(cacheKey);
        if (cachedUser is not null)
        {
            await next(context);
            return;
        }

        var user = await service.FindByIdAsync(guid);
        user ??= await CreateUserAsync(context, service, guid);
        await cache.SetStringAsync(cacheKey, user.Login, options);
        await next(context);
    }

    private static async Task<User> CreateUserAsync(HttpContext context, IUserService service, Guid guid)
    {
        // NOTE(W2): Here we can sync things from the OpenIDConnect provider if necessary.
        string login = context.User.GetPreferredUsername()?.Value ?? RandomUserName(guid);
        return await service.CreateAsync(new()
        {
            Id = guid,
            Login = login,
            DisplayName = login
        });
    }

    private DistributedCacheEntryOptions options = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) };

    private static string RandomUserName(Guid id)
    {
        return $"{new Faker().Hacker.Noun()}_{id.ToString().Split('-')[0]}";
    }
}
