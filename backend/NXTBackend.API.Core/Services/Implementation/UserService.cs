using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class UserService(DatabaseContext ctx) : BaseService<User>(ctx), IUserService
{
    /// <inheritdoc/>
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
    }

    /// <inheritdoc/>
    public async Task<User?> FindByNameAsync(string displayName)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.DisplayName == displayName);
    }

    /// <inheritdoc/>
    public Task<User> SubscribeToCursus(User entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<Review?> didRecentReviewOnUserProject(User entity, UserProject userProject)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> SubscribeToCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> UnsubscribeFromCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<PaginatedList<Notification>> GetEvents(User entity)
    {
        throw new NotImplementedException();
    }
}
