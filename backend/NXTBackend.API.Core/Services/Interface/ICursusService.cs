
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Domain;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Services.Interface;
public interface ICursusService : IDomainService<Cursus>, ICollaborative<Cursus>
{
    /// <summary>
    /// Extract all the goal IDs inside the trakc
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public IEnumerable<Guid> ExtractTrackGoals(CursusTrack track);

    /// <summary>
    /// Takes in a <see cref="IGraphTrack"/> compliant datatype and returns the goal IDs and the constructed
    /// GraphNode.
    /// </summary>
    /// <exception cref="ServiceException"/> Unable to process the request.
    /// <param name="track"></param>
    /// <returns></returns>
    public Task<CursusTrackDTO> ConstructTrack(CursusTrack track);
}
