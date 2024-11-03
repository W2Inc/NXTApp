using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
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
