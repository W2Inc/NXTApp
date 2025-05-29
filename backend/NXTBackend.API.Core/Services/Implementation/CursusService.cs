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
using NXTBackend.API.Models.Shared;
using NXTBackend.API.Models.Shared.Graph;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService : BaseService<Cursus>, ICursusService
{
    private readonly IGoalService _goalService;

    public CursusService(DatabaseContext ctx, IGoalService goalService) : base(ctx)
    {
        _goalService = goalService;
    }

    public async Task<CursusTrackDTO> ConstructTrack(CursusTrack track)
    {
        var goalIds = ExtractTrackGoals(track).Distinct().ToArray();
        var goals = await _context.LearningGoals
            .AsNoTracking()
            .Where(g => goalIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id);

        return new CursusTrackDTO()
        {
            Root = BuildNode(track.Goals, track.Next, 1)
        };

        TrackNodeDTO BuildNode(Guid[] ids, CursusTrack[] nextNodes, int nodeId)
        {
            var nodeGoals = ids
                .Select(id =>
                {
                    var goal = goals[id];
                    return new TrackGoalDTO()
                    {
                        Id = goal.Id,
                        Name = goal.Name,
                        State = TaskState.Active
                    };
                })
                .ToArray();
    
            var children = nextNodes
                .Select((node, index) => BuildNode(node.Goals, node.Next, nodeId * 100 + index + 1))
                .ToArray();
    
            return new TrackNodeDTO()
            {
                Id = nodeId,
                Children = children,
                Goals = nodeGoals,
            };
        }
    }

    /// <inheritdoc />
    public IEnumerable<Guid> ExtractTrackGoals(CursusTrack track)
    {
        // Start the recursion with the track data
        var ids = CollectGoalIds(track);
        var distinctCount = ids.Distinct().Count();
        if (ids.Count() != distinctCount)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Duplicate goals detected in track");
        return ids;

        static IEnumerable<Guid> CollectGoalIds(CursusTrack data, int depth = 0)
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
    }

    /// <summary>
    /// Simply marks the cursus as deprecated
    /// </summary>
    public override async Task<Cursus> DeleteAsync(Cursus entity)
    {
        // If no references exist, we can't hard delete.
        entity.Deprecated = true;
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
        var cursus = await _dbSet
            .Include(c => c.Collaborators)
            .FirstOrDefaultAsync(c => c.Id == cursusId);
        
        if (cursus is null)
            return false;

        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            return false;

        if (cursus.Collaborators.All(c => c.Id != userId))
            cursus.Collaborators.Add(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCollaborator(Guid cursusId, Guid userId)
    {
        var cursus = await _dbSet
            .Include(c => c.Collaborators)
            .FirstOrDefaultAsync(c => c.Id == cursusId);

        var user = cursus?.Collaborators.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return false;

        cursus?.Collaborators.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
