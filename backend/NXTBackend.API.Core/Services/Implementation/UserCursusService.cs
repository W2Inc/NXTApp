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
        // Get the user's cursus with track data in a single query
        var userCursus = await _context.UserCursi
            .Include(uc => uc.Cursus)
            .Where(uc => uc.CursusId == cursusId && uc.UserId == userId)
            .FirstOrDefaultAsync();

        if (userCursus?.Cursus.Track is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "Not found");
            
        var track = JsonSerializer.Deserialize<CursusTrack>(userCursus.Cursus.Track)
            ?? throw new ServiceException(StatusCodes.Status500InternalServerError, "Failed to deserialize track data");

        // Get all goal IDs in the track
        var allGoalIds = GetGoalIDs(track).Distinct().ToList();
        
        // Perform a single complex query to get all the required data
        var goalsQuery = await _context.LearningGoals
            .Where(g => allGoalIds.Contains(g.Id))
            .Select(g => new
            {
                Goal = g,
                Projects = g.Projects.Select(p => new
                {
                    ProjectId = p.Id,
                    UserProjects = p.UserProjects
                        .Where(up => up.Members.Any(m => m.UserId == userId))
                        .Select(up => new { up.Id, up.State })
                        .ToList()
                }).ToList()
            })
            .ToListAsync();
        
        // Map to create goal state dictionary
        var goalStates = new Dictionary<Guid, TaskState>();
        var goalNames = new Dictionary<Guid, string>();
        
        foreach (var goalData in goalsQuery)
        {
            goalNames[goalData.Goal.Id] = goalData.Goal.Name;
            
            // Determine state based on related projects
            var projectStates = goalData.Projects
                .SelectMany(p => p.UserProjects.Select(up => up.State))
                .ToList();
            
            TaskState state;
            if (projectStates.Count == 0)
            {
                state = TaskState.Inactive;
            }
            else if (projectStates.All(s => s == TaskState.Completed))
            {
                state = TaskState.Completed;
            }
            else if (projectStates.Any(s => s == TaskState.Active))
            {
                state = TaskState.Active;
            }
            else
            {
                state = TaskState.Inactive;
            }
            
            goalStates[goalData.Goal.Id] = state;
        }
        
        // Ensure all goal IDs have a state
        foreach (var goalId in allGoalIds.Where(id => !goalStates.ContainsKey(id)))
            goalStates[goalId] = TaskState.Inactive;
        
        return new CursusTrackDTO
        {
            Root = BuildTrackNode(track.Goals, track.Next)
        };

        // Local function to build a track node and its children recursively
        TrackNodeDTO BuildTrackNode(Guid[] goalIds, CursusTrack[] nextNodes, int nodeId = 1)
        {
            // Create goal DTOs with state information
            var goalDtos = goalIds.Select(goalId => new TrackGoalDTO
            {
                Id = goalId,
                Name = goalNames.GetValueOrDefault(goalId, "Unknown Goal"),
                State = goalStates.GetValueOrDefault(goalId, TaskState.Inactive)
            }).ToArray();

            // Build child nodes recursively
            var childNodes = nextNodes.Select((node, index) => 
                BuildTrackNode(node.Goals, node.Next, nodeId * 10 + index + 1)).ToArray();

            return new TrackNodeDTO
            {
                Id = nodeId,
                Goals = goalDtos,
                Children = childNodes
            };
        }
    }
    
    private IEnumerable<Guid> GetGoalIDs(CursusTrack data, int depth = 0)
    {
        // TODO: Move these as statics somewhere ?
        const int maxDepth = 255;
        const int maxNextNodes = 4;

        // Handle null or empty arrays
        if (data?.Goals is not null)
            foreach (var goal in data.Goals)
                yield return goal;

        if (data?.Next is null)
            yield break;
        
        foreach (var node in data.Next)
        {
            if (depth > maxDepth)
            {
                throw new ServiceException(StatusCodes.Status422UnprocessableEntity,
                    "Graph too deep (exceeds maximum depth)");
            }
                
            if (node.Next is not null && node.Next.Length > maxNextNodes)
            {
                throw new ServiceException(StatusCodes.Status422UnprocessableEntity,
                    "Node has too many child nodes (exceeds maximum)");
            }
                
            foreach (var goalId in GetGoalIDs(node, depth + 1))
            {
                yield return goalId;
            }
        }
    }
}