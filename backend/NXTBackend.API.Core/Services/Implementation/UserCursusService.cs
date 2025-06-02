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
        // Check if such user cursus exists...
        var userCursus = await Query()
            .Include(uc => uc.Cursus).AsNoTracking()
            .Include(uc => uc.UserGoals)
            .ThenInclude(ug => ug.Goal)
            .ThenInclude(g => g.Projects)
            .ThenInclude(gp => gp.Project)
            .ThenInclude(p => p.UserProjects)
            .FirstOrDefaultAsync();

        if (userCursus is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "UserCursus not found");
        if (userCursus.LastComputeAt is null)
        {
            userCursus.LastComputeAt = DateTimeOffset.Now;
            userCursus.Track = await ComputeGraph(userCursus);
            await UpdateAsync(userCursus);
        }
        _logger.LogInformation("{Projects}", userCursus.UserGoals);

        await BuildGraph(userCursus);

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

        return new CursusTrackDTO();
    }

    /// <summary>
    /// Compute the graph.
    /// </summary>
    /// <remarks>
    /// 
    /// In essence the following steps:
    /// - Recurse through each node of the base cursus track
    /// - Per node:
    /// - - Determine goals
    /// - - - Check if user has a user goal of any of the goals reference inside the node
    /// - - - If true:
    /// - - - - Check the state of any node, if all inactive user hasn't reached yet (thus use that node as reference)
    /// - - - - Otherwise refer to the snapshot (a.k.a what goals are reference in the userCursus)
    /// - - - If false
    /// - - - - Use cursus track goals as reference 
    /// - - Determine state:
    /// - - - If all projects inside are inactive, then inactive
    /// - - - If all projects inside are active or some are in other various states, still active.
    /// - - - If all projects are completed, then goal is completed.
    /// </remarks>
    /// <param name="userCursus"></param>
    private async Task<string> ComputeGraph(UserCursus userCursus)
    {
        userCursus.LastComputeAt = DateTime.UtcNow;
        var cursusTrack = JsonSerializer.Deserialize<CursusTrack>(userCursus.Cursus.Track);
        if (cursusTrack is null)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Invalid cursus track");

        // Get user track if exists, or create empty one
        var userTrack = userCursus.Track is not null
            ? JsonSerializer.Deserialize<CursusTrack>(userCursus.Track)
            : new CursusTrack { Goals = [], Next = [] };

        return JsonSerializer.Serialize(Compute(cursusTrack, userTrack));

        CursusTrack Compute(CursusTrack cursusNode, CursusTrack userNode)
        {
            // Determine goals to use
            var goals = DetermineGoalsToUse(cursusNode.Goals, userNode.Goals);

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
                Goals = goals,
                Next = nextNodes.ToArray()
            };
        }

        Guid[] DetermineGoalsToUse(Guid[] cursusGoalIds, Guid[] userGoalIds)
        {
            // Check if user has any of these goals
            var userGoals = userCursus.UserGoals.Where(ug => cursusGoalIds.Contains(ug.GoalId)).ToList();
            if (userGoals.Count == 0)
                return cursusGoalIds;

            // Determine based on state
            var allInactive = userGoals.All(ug => DetermineGoalState(ug.Goal) is TaskState.Inactive);
            if (allInactive)
                return cursusGoalIds;
            return userGoalIds.Length > 0 ? userGoalIds : cursusGoalIds;
        }

        TaskState DetermineGoalState(LearningGoal goal)
        {
            if (goal.Projects.Count == 0)
                return TaskState.Completed;

            var allInactive = true;
            var allCompleted = true;

            foreach (var goalProject in goal.Projects)
            {
                var project = goalProject.Project;
                var userProject = project.UserProjects.FirstOrDefault(up => up.Members.Any(m => m.UserId == userCursus.UserId));
                
                if (userProject == null)
                {
                    // Project not started by user
                    allCompleted = false;
                }
                else
                {
                    // User has this project
                    allInactive = false;

                    if (userProject.State != TaskState.Completed)
                    {
                        // Not completed yet
                        allCompleted = false;
                    }
                }
            }

            if (allInactive)
                return TaskState.Inactive;
            return allCompleted ? TaskState.Completed : TaskState.Active;
        }
    }

    private async Task BuildGraph(UserCursus cursus)
    {
    }
}