using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;

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

    /// <summary>
    /// Upserts the user details of a user
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <param name="details">The details to put</param>
    /// <returns>The newly updated user</returns>
    public Task<User?> UpsertDetails(Guid id, Details details);

    /// <summary>
    /// Get the filtered Spotlight events for user.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<SpotlightEvent>> GetSpotlights(Guid id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="spotlightId"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task<SpotlightEventAction> SetSpotlight(Guid userId, Guid spotlightId, bool action);
}
