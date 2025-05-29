// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Models.Shared;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface;
public interface IUserCursusService : IDomainService<UserCursus>
{
    /// <summary>
    /// Takes in a <see cref="IGraphTrack"/> compliant datatype and returns the goal IDs and the constructed
    /// GraphNode.
    /// </summary>
    /// <exception cref="ServiceException"/> Unable to process the request.
    /// <param name="track"></param>
    /// <returns></returns>
    public Task<CursusTrackDTO> ConstructTrack(Guid userId, Guid cursusId);
}
