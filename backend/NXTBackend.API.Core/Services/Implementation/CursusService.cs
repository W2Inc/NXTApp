using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService : BaseService<Cursus>, ICursusService
{
    private readonly IGoalService _goalService;

    public CursusService(DatabaseContext ctx, IGoalService goalService) : base(ctx)
    {
        _goalService = goalService;
    }

    /// <summary>
    /// Validates, processes, and converts a CursusTrackPutRequestDTO to a GraphNode
    /// </summary>
    /// <param name="data">The DTO containing track data</param>
    /// <returns>A tuple containing the processed GraphNode and its serialized representation</returns>
    /// <exception cref="InvalidOperationException">Thrown when validation fails</exception>
    // public async Task<GraphNode> ValidateTrackData(IGraphTrack data)
    // {

    // }

    public async Task<GraphNode> ConstructTrack(IGraphTrack track)
    {
        var goalIds = ExtractTrackGoals(track).Distinct().ToArray();
        var goals = await _context.LearningGoals.AsNoTracking()
            .Where(g => goalIds.Contains(g.Id))
            .ToListAsync();

        var goalDict = goals.ToDictionary(g => g.Id);
        GraphNode BuildNode(Guid[] goalIds, GraphNodeData[] nextNodes, int nodeId)
        {
            var nodeGoals = goalIds
                .Select(id =>
                {
                    var goal = goalDict[id];
                    return new GraphNodeGoal(
                        goal.Id,
                        goal.Name,
                        goal.Description,
                        TaskState.Active // TODO: Evaluate state based on parent,
                    );
                })
                .ToArray();

            var children = nextNodes
                .Select((node, index) => BuildNode(node.Goals, node.Next, nodeId * 100 + index + 1))
                .ToArray();

            return new GraphNode(nodeId, nodeGoals, children);
        }

        return BuildNode(track.Goals, track.Next, 1);
    }

    /// <inheritdoc />
    public IEnumerable<Guid> ExtractTrackGoals(IGraphTrack track)
    {
        static IEnumerable<Guid> CollectGoalIds(GraphNodeData data, int depth = 0)
        {
            foreach (var goal in data.Goals)
                yield return goal;
            foreach (var node in data.Next)
            {
                if (depth > 255)
                    throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Graph too large");
                if (node.Next.Length > 4)
                    throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "A node has too many goals");
                foreach (var goalId in CollectGoalIds(node, depth))
                    yield return goalId;
            }
        }

        // Start the recursion with the track data
        var ids = CollectGoalIds(new GraphNodeData(track.Goals, track.Next));
        var distinctCount = ids.Distinct().Count();
        if (ids.Count() != distinctCount)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Duplicate goals detected in track");
        return ids;
    }

    /// <summary>
    /// Depending on the state of the cursus either does a "soft delete" or
    /// a hard delete.
    /// </summary>
    /// <remarks>
    /// If a cursus has no references to any goals, projects etc it can be
    /// safely hard deleted. However in most cases this would lead to a
    /// very messed up state of models referencing a cursus that is gone.
    ///
    /// Furthermore for curriculums, a user might want to see their old cursus
    /// even if it is considered "old and stale".
    /// </remarks>
    public override async Task<Cursus> DeleteAsync(Cursus entity)
    {
        // If no references exist, we can't hard delete.
        entity.Enabled = false;
        if (entity.UserCursi.Count < 1)
            _dbSet.Remove(entity);
        else
            await UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<(Cursus?, User?)> IsCollaborator(Guid entityId, Guid userId)
    {
        var query = from c in _dbSet.AsNoTracking()
                    where c.Id == entityId
                    select new
                    {
                        Cursus = c,
                        Collaborator = c.CreatorId == userId ? c.Creator : c.Collaborators.FirstOrDefault(co => co.Id == userId)
                    };

        var result = await query.FirstOrDefaultAsync();
        return (result?.Cursus, result?.Collaborator);
    }

    public async Task<bool> AddCollaborator(Guid cursusId, Guid userId)
    {
        var cursus = await _dbSet.Include(c => c.Collaborators)
                                .FirstOrDefaultAsync(c => c.Id == cursusId);
        if (cursus is null)
            return false;

        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            return false;

        if (!cursus.Collaborators.Any(c => c.Id == userId))
            cursus.Collaborators.Add(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCollaborator(Guid cursusId, Guid userId)
    {
        var cursus = await _dbSet.Include(c => c.Collaborators)
                                .FirstOrDefaultAsync(c => c.Id == cursusId);
        if (cursus is null)
            return false;

        var user = cursus.Collaborators.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return false;

        cursus.Collaborators.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
