using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface INotificationService : IDomainService<Notification>
{
    /// <summary>
    /// Find the feature by its name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns> The feature or null</returns>
    public Task<Notification?> FindByTitleAsync(string name);

}
