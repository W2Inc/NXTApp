using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class GoalService : IGoalService
{
    private readonly DatabaseContext _databaseContext;

    public GoalService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<LearningGoal?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.LearningGoals.FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<LearningGoal> CreateAsync(LearningGoal entity)
    {
        var goal = await _databaseContext.LearningGoals.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    /// <inheritdoc />
    public async Task<LearningGoal> UpdateAsync(LearningGoal entity)
    {
        var goal = _databaseContext.LearningGoals.Update(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    /// <inheritdoc />
    public async Task<LearningGoal> DeleteAsync(LearningGoal entity)
    {
        var goal = _databaseContext.LearningGoals.Remove(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<LearningGoal>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.LearningGoals.Select(goal => goal);
        return await PaginatedList<LearningGoal>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);
    }

    /// <inheritdoc />
    public async Task<LearningGoal> AddProject(LearningGoal goal, Project project)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<LearningGoal> RemoveProject(LearningGoal goal, Project project)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Project>> GetProjects(LearningGoal goal, PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<PaginatedList<User>> GetUsers(LearningGoal goal, PaginationParams pagination)
    {
        throw new NotImplementedException();
    }
}