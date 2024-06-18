namespace NXTBackend.API.Core.Services.Interface;

using NXTBackend.API.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Requests;

public interface IUserService : IDomainService<User>
{
    /// <summary>
    /// Find the user by its login.
    /// </summary>
    /// <param name="login">The login.</param>
    /// <returns>The user.</returns>
    public Task<User?> FindByLoginAsync(string login);

    /// <summary>
    /// Find the user by its display name.
    /// </summary>
    /// <param name="displayName">The Display name</param>
    /// <returns>The user.</returns>
    public Task<User?> FindByNameAsync(string displayName);

    public Task<User> SubscribeToCursus(User entity);
}