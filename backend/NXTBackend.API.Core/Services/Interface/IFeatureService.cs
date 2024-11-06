using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IFeatureService : IDomainService<Feature>
{
    /// <summary>
    /// Find the feature by its name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns> The feature or null</returns>
    public Task<Feature?> FindByNameAsync(string name);

}
