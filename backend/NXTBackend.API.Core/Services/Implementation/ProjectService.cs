using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
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
			markdown = await _git.GetRawFileContent(project.GitInfo.Namespace, file, branch);
			await _cache.SetStringAsync(cacheKey, markdown, options: new ()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
			});
		}

		return markdown;
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
}
