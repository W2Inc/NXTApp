using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class ResourceOwnerService(IUserService userService) : IResourceOwnerService
{
    public async Task<ResourceOwner?> FindByIdAsync(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is not null)
            return new(user, null, OwnerKind.User);
        return null; // TODO: Implement organizations overall...
    }
}
