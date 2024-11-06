using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
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
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cursus"></param>
    /// <returns></returns>
    public Task<User> UpdateDetails(User entity, Details details);

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cursus"></param>
    /// <returns></returns>
    public Task<User> SubscribeToCursus(User entity, Cursus cursus);

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cursus"></param>
    /// <returns></returns>
    public Task<User> UnsubscribeFromCursus(User entity, Cursus cursus);

    /// <summary>
    /// Get all the events of the user
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<PaginatedList<Notification>> GetNotifications(User entity);

    /// <summary>
    /// Dismiss a event for a user
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="notification"></param>
    /// <returns></returns>
    public Task<NotificationAction> DismissNotification(User entity, Notification notification);

    /// <summary>
    /// Check if the user recently reviewed the project instance, recently means within the last 24 hours
    /// </summary>
    /// <param name="userProject"></param>
    /// <returns></returns>
    public Task<Review?> didRecentReviewOnUserProject(User entity, UserProject userProject);
}
