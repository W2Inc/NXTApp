using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;

namespace NXTBackend.API;

public static class Extensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
            return null;

        return Guid.TryParse(userId.Value, out var guid) ? guid : null;
    }

}
