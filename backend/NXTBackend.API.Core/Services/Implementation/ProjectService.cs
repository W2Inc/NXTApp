using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Core.Utils.Query;
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
    private readonly IGitService _git;
    private readonly IDistributedCache _cache;

    public ProjectService(DatabaseContext ctx, IGitService git, IDistributedCache cache) : base(ctx)
    {
        _git = git ?? throw new ArgumentNullException(nameof(git));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));

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

    /// <inheritdoc />
    public async Task<string> GetFileFromProject(Guid projectId, string file, string branch)
    {
        var project = await _context.Projects
            .Include(p => p.GitInfo)
            .FirstOrDefaultAsync(p => p.Id == projectId)
                ?? throw new ServiceException(StatusCodes.Status404NotFound, "Project not found");

        var cacheKey = $"{project.Id}:{branch}:{file}";
        var markdown = await _cache.GetStringAsync(cacheKey);
        if (markdown is null)
        {
            markdown = await _git.GetFile(project.GitInfoId, file, branch);
            await _cache.SetStringAsync(cacheKey, markdown, options: new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

		return Encoding.UTF8.GetString(Convert.FromBase64String(markdown));
    }

    public Task<PaginatedList<Rubric>> GetRubric(Project project, PaginationParams pagination)
    {
        // Implementation using _context
        throw new NotImplementedException();
    }

    public Task<PaginatedList<User>> GetUsers(Project project, PaginationParams pagination, SortingParams sorting)
    {
        // Implementation using _context
        throw new NotImplementedException();
    }

    public override async Task<Project> DeleteAsync(Project entity)
    {
        await _git.UpdateRepository(entity.GitInfoId, new()
        {
            Archived = true
        });

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<(Project?, User?)> IsCollaborator(Guid entityId, Guid userId)
    {
        var project = await FindByIdAsync(entityId);
        if (project is null)
            return (null, null);
        // TODO: Do we want to count the creator always as a collaborator ?
        if (project.CreatorId == userId)
            return (project, project.Creator);

        var (git, user) = await _git.IsCollaborator(project.GitInfoId, userId);
        if (git is null)
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Project has no linked git repository");
        return (project, user);
    }

    public Task<bool> RemoveCollaborator(Guid entityId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddCollaborator(Guid entityId, Guid userId)
    {
        throw new NotImplementedException();
    }
}
