using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Shared;
using NXTBackend.API.Models.Shared.Graph;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Service responsible for managing user cursus and related tracks/goals.
/// </summary>
public sealed class UserCursusService : BaseService<UserCursus>, IUserCursusService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCursusService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserCursusService(DatabaseContext context) : base(context)
    {
    }
    
    /// <summary>
    /// Constructs the track for a user's cursus with the current state of all goals.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cursusId">The cursus ID.</param>
    /// <returns>A populated cursus track DTO with goal states.</returns>
    /// <exception cref="ServiceException">Thrown when the cursus track is not found or cannot be processed.</exception>
    public async Task<CursusTrackDTO> ConstructTrack(Guid userId, Guid cursusId)
    {
        // Check if such user cursus exists...
        var userCursus = await  Query(false)
            .Where(u => u.UserId == userId && u.CursusId == cursusId)
            .FirstOrDefaultAsync();
        
        if (userCursus is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "UserCursus not found");
        
        // Does graph need to be re-computed since the user made any progress?
        // If true, compute
        // If false, serve snapshot track
        
        // If computed:
        // - Fetch the original cursus
        // - Compare tracks
        // - - When comparing the tracks, the snapshot takes precedence
        // - - When cursus was e.g: Goal A -> Goal B during the last snapshot but is now: Goal C -> Goal B
        // - - - Then we prefer the snapshot as that is what the user actually did, but we need to see state of goal to determine precedence
        // - - - If Goal state is inactive, cursus wins else userCursus Snapshot wins (Optional Behaviour: Always prefer snapshot one)
        await ComputeGraph(userCursus);

        return new CursusTrackDTO();
    }

    private async Task ComputeGraph(UserCursus cursus)
    {
        
    }
}