using NXTBackend.API.Domain.Entities.Notification;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface ISpotlightEventService : IDomainService<SpotlightEvent>
{
    /// <summary>
    /// Find the feature by its name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns> The feature or null</returns>
    public Task<SpotlightEvent?> FindByTitleAsync(string name);

}
