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
public sealed class ProjectService(DatabaseContext ctx) : BaseService<Project>(ctx), IProjectService
{
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