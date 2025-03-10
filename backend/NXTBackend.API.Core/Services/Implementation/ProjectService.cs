using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class ProjectService : BaseService<Project>, IProjectService
{

    public ProjectService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<string>("slug", (q, slug) => q.Where(p => p.Slug == slug));
        DefineFilter<string>("name", (q, name) => q.Where(p => EF.Functions.Like(p.Name, $"%{name}%")));
    }

    public async Task<Project> CreateProjectWithGit(Project project, Git git)
    {
        var newProject = await _context.Projects.AddAsync(project);
        newProject.Entity.GitInfoId = git.Id;
        await _context.SaveChangesAsync();
        return newProject.Entity;
    }

    public Task<Git> GetGitInfo(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<Rubric>> GetRubric(Project project, PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<User>> GetUsers(Project project, PaginationParams pagination, SortingParams sorting)
    {
        throw new NotImplementedException();
    }
}
