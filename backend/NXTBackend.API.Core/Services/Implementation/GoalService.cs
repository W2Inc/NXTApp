// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Joins;
using NXTBackend.API.Infrastructure.Database;

// ============================================================================

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class GoalService : BaseService<LearningGoal>, IGoalService
{
    public GoalService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("id", (q, id) => q.Where(g => g.Id == id));
        DefineFilter<string>("name", (q, name) => q.Where(g => EF.Functions.Like(g.Name, $"%{name}%")));
        DefineFilter<string>("slug", (q, slug) => q.Where(g => EF.Functions.Like(g.Slug, $"%{slug}%")));
    }

    /// <inheritdoc />
    public async Task<(LearningGoal?, User?)> IsCollaborator(Guid entityId, Guid userId)
    {
        var result = await _context.GoalCollaborator
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.Goal)
            .Where(e => e.GoalId == entityId && e.UserId == userId)
            .FirstOrDefaultAsync();

        return (result?.Goal, result?.User);
    }

    public async Task<bool> AddCollaborator(Guid goalId, Guid userId)
    {
        var (_, user) = await IsCollaborator(goalId, userId);
        if (user is not null)
            return false;

        // Ensure entities exists
        var goal = await FindByIdAsync(goalId);
        if (goal is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "Goal not found");

        user = await _context.Users.FindAsync(userId);
        if (user is null)
            throw new ServiceException(StatusCodes.Status404NotFound, "User not found");

        await _context.CursusCollaborator.AddAsync(new()
        {
            CursusId = goal.Id,
            UserId = userId,
        });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCollaborator(Guid goalId, Guid userId)
    {
        var result = await _context.GoalCollaborator
            .AsNoTracking()
            .Where(e => e.GoalId == goalId && e.UserId == userId)
            .FirstOrDefaultAsync();

        if (result is null)
            return false;

        _context.GoalCollaborator.Remove(result);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<User>> GetUsers(
        LearningGoal goal,
        PaginationParams pagination,
        SortingParams sorting
    )
    {
        var query = _context.Users.AsNoTracking().Where(u => u.UserGoals.Any(g => g.GoalId == goal.Id));
        return await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Cursus>> GetCursi(LearningGoal goal, PaginationParams pagination,
        SortingParams sorting)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Project>> GetProjects(LearningGoal goal, SortingParams sorting)
    {
        var projects = _context.GoalProject.AsNoTracking()
            .Where(gp => gp.GoalId == goal.Id)
            .Select(gp => gp.Project);

        return await SortedList<Project>.Apply(projects, sorting).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Project>> SetProjects(LearningGoal goal, IEnumerable<Guid> projects)
    {
        var projectIds = projects.ToList();
        if (projectIds.Count > 4)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Maximum number of projects exceeded");

        var existingProjects = await _context.Projects
            .Where(p => projectIds.Contains(p.Id))
            .ToListAsync();

        if (existingProjects.Count != projectIds.Count)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Non-existent projects");

        var existingRelations = await _context.GoalProject
            .Where(gp => gp.GoalId == goal.Id && projectIds.Contains(gp.ProjectId))
            .Select(gp => gp.ProjectId)
            .ToListAsync();

        if (existingRelations.Count is not 0)
            throw new ServiceException(StatusCodes.Status409Conflict, "Some projects were already associated with this goal");

        var goalProjects = projectIds.Select(projectId => new GoalProject
        {
            GoalId = goal.Id,
            ProjectId = projectId,
        });

        await _context.GoalProject.AddRangeAsync(goalProjects);
        await _context.SaveChangesAsync();
        return existingProjects;
    }
}
