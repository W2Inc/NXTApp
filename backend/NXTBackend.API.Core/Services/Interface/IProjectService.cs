﻿
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;
public interface IProjectService : IDomainService<Project>, ICollaborative<Project>
{
    /// <summary>
    /// Create a new project with a specific git model assigned to it.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="git"></param>
    /// <returns></returns>
    public Task<Project> CreateProjectWithGit(Project project, Git git);

    /// <summary>
    /// Get all the rubric that are assigned to a project.
    /// </summary>
    /// <param name="project"> The project to get the rubric from. </param>
    /// <param name="pagination"> The pagination parameters. </param>
    /// <returns>The list of rubric that are assigned to the project.</returns>
    public Task<PaginatedList<Rubric>> GetRubric(Project project, PaginationParams pagination);

    /// <summary>
    /// Get all the users that are doing this project.
    /// </summary>
    /// <param name="pagination"> The pagination parameters. </param>
    /// <returns></returns>
    public Task<PaginatedList<User>> GetUsers(Project project, PaginationParams pagination, SortingParams sorting);
}
