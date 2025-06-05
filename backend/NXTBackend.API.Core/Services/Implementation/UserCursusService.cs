using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserCursusService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCursusService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger"></param>
    public UserCursusService(DatabaseContext context, ILogger<UserCursusService> logger) : base(context)
    {
        _logger = logger;
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
        // 1. First, load just the minimal UserCursus data needed without tracking
        var userCursus = await _context.UserCursi
            .AsNoTracking()
            .Where(uc => uc.UserId == userId && uc.CursusId == cursusId)
            .Select(uc => new { uc.Id, uc.Track, uc.LastComputeAt })
            .FirstOrDefaultAsync();

        if (userCursus is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "UserCursus not found");

        var newTrack = userCursus.Track;
        if (userCursus.LastComputeAt is not null && newTrack is not null)
            return await BuildGraph(newTrack);
        
        // 2. Load cursus separately without tracking
        var cursus = await _context.Cursi
            .AsNoTracking()
            .Where(c => c.Id == cursusId)
            .Select(c => new { c.Track })
            .FirstOrDefaultAsync();

        if (cursus is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "Cursus not found");

        // 3. Load user goals separately with minimal data needed
        var userGoalsInfo = await _context.UserGoals
            .AsNoTracking()
            .Where(ug => ug.UserCursusId == userCursus.Id)
            .Select(ug => new
            {
                ug.GoalId,
                GoalState = ug.Goal.Projects.Count == 0 ? TaskState.Completed :
                    ug.Goal.Projects.Any(gp =>
                        gp.Project.UserProjects.Any(up =>
                            up.Members.Any(m => m.UserId == userId) &&
                            up.State != TaskState.Inactive)) ? TaskState.Active :
                    TaskState.Inactive
            })
            .ToDictionaryAsync(x => x.GoalId, x => x.GoalState);

        // 5. Compute new track without tracking entities
        var cursusTrack = JsonSerializer.Deserialize<CursusTrack>(cursus.Track);
        var userTrack = userCursus.Track is not null
            ? JsonSerializer.Deserialize<CursusTrack>(userCursus.Track)
            : new CursusTrack { Goals = [], Next = [] };

        // 6. Compute the track with our improved algorithm
        newTrack = ComputeTrack(cursusTrack, userTrack, userGoalsInfo);

        // 7. Update only the required fields in the database directly
        // Using EF Core 7.0+ ExecuteUpdateAsync to avoid tracking
        await _context.UserCursi
            .Where(uc => uc.Id == userCursus.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(uc => uc.Track, newTrack)
                .SetProperty(uc => uc.LastComputeAt, DateTime.UtcNow));

        // 8. Build and return the DTO without tracking changes
        return await BuildGraph(newTrack);
    }

// Separate pure function for track computation
    private string ComputeTrack(CursusTrack cursusTrack, CursusTrack userTrack,
        Dictionary<Guid, TaskState> userGoalStates)
    {
        var computedTrack = Compute(cursusTrack, userTrack);
        return JsonSerializer.Serialize(computedTrack);

        CursusTrack Compute(CursusTrack cursusNode, CursusTrack userNode)
        {
            // Process goals by position
            var computedGoals = DetermineGoalsToUse(cursusNode, userNode);

            // Compute next nodes recursively
            var nextNodes = new List<CursusTrack>();
            for (var i = 0; i < cursusNode.Next.Length; i++)
            {
                var cursusNext = cursusNode.Next[i];
                var userNext = (userNode.Next != null && i < userNode.Next.Length)
                    ? userNode.Next[i]
                    : new CursusTrack { Goals = [], Next = [] };

                nextNodes.Add(Compute(cursusNext, userNext));
            }

            // Return computed track node
            return new CursusTrack
            {
                Goals = computedGoals,
                Next = nextNodes.ToArray()
            };
        }

        Guid[] DetermineGoalsToUse(CursusTrack cursusNode, CursusTrack userNode)
        {
            var resultGoals = new List<Guid>();

            // Get the total number of goals in the cursus node
            int goalCount = cursusNode.Goals.Length;

            // Process each goal position
            for (int i = 0; i < goalCount; i++)
            {
                var cursusGoalId = cursusNode.Goals[i];

                // Check if the user has a goal at this position
                if (userNode.Goals != null && i < userNode.Goals.Length)
                {
                    var userGoalId = userNode.Goals[i];

                    // Check if this user goal is active or completed using the preloaded dictionary
                    if (userGoalStates.TryGetValue(userGoalId, out var goalState) &&
                        (goalState == TaskState.Active || goalState == TaskState.Completed))
                    {
                        // User has started this goal, keep the user's goal
                        resultGoals.Add(userGoalId);
                        _logger.LogDebug(
                            "Keeping active user goal {UserGoalId} instead of cursus goal {CursusGoalId} at position {Position}",
                            userGoalId, cursusGoalId, i);
                        continue;
                    }
                }

                // If we reach here, use the cursus goal
                resultGoals.Add(cursusGoalId);
                _logger.LogDebug("Using cursus goal {CursusGoalId} at position {Position}", cursusGoalId, i);
            }

            return resultGoals.ToArray();
        }
    }

    // Separate method to build the graph DTO without modifying entities
    private async Task<CursusTrackDTO> BuildGraph(string trackJson)
    {
        // Implement this to build your DTO without modifying any tracked entities
        // Load any data needed with AsNoTracking()

        // For example:
        var track = JsonSerializer.Deserialize<CursusTrack>(trackJson);
        // ... build your DTO from the track ...

        return new CursusTrackDTO();
    }
}