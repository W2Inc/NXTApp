using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface ISpotlightEventActionService : IDomainService<SpotlightEventAction>
{
    /// <summary>
    /// Sets the action status of a spotlight event for a user.
    /// </summary>
    /// <param name="spotlight">For wich spotlight are we setting the action</param>
    /// <param name="action">If true, the spotlight is dismissed and not shown again.</param>
    /// <returns>The spotlightEventAction</returns>
    public Task<SpotlightEventAction> Upsert(User user, SpotlightEvent spotlight, bool action);
}
