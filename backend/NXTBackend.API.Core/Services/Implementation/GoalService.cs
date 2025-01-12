using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class GoalService(DatabaseContext ctx) : BaseService<LearningGoal>(ctx), IGoalService
{
    /// <inheritdoc />
    public async Task<LearningGoal> AddProject(LearningGoal goal, Project project)
    {
        if (goal.Projects.Contains(project))
            throw new ServiceException("Project is already a part of goal");
        goal.Projects.Add(project);
        await _context.SaveChangesAsync();
        return goal;
    }

    /// <inheritdoc />
    public async Task<LearningGoal> RemoveProject(LearningGoal goal, Project project)
    {
        if (!goal.Projects.Remove(project))
            throw new ServiceException("Project is not part of goal");
        await _context.SaveChangesAsync();
        return goal;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<User>> GetUsers(LearningGoal goal, PaginationParams pagination, SortingParams sorting)
    {
        var query = _context.Users.AsNoTracking().Where(u => u.UserGoals.Any(g => g.GoalId == goal.Id));
        return await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Project>> GetProjects(LearningGoal goal, PaginationParams pagination, SortingParams sorting)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Include(p => p.Creator)
            .Include(p => p.GitInfo)
            .Where(p => p.Goals.Any(g => g.Id == goal.Id));

        return await PaginatedList<Project>.CreateAsync(query, pagination.Page, pagination.Size);
    }
}
