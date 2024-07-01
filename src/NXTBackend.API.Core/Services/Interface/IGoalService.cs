
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.User;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;
public interface IGoalService : IDomainService<LearningGoal>
{
    /// <summary>
    /// Remove a project from a learning goal.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    Task<LearningGoal> RemoveProject(LearningGoal goal, Project project);

    /// <summary>
    /// Add a project to a learning goal.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    Task<LearningGoal> AddProject(LearningGoal goal, Project project);

    /// <summary>
    /// Get all the projects that are part of a learning goal.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<PaginatedList<Project>> GetProjects(LearningGoal goal, PaginationParams pagination);

    /// <summary>
    /// Get all the users that are doing this learning goal.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<PaginatedList<User>> GetUsers(LearningGoal goal, PaginationParams pagination);
}