using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class UserProjectService : BaseService<UserProject>, IUserProjectService
{
    public UserProjectService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("project_id", (q, projectId) => q.Where(p => p.ProjectId == projectId));
        DefineFilter<Guid>("user_id", (q, userId) => q.Include(up => up.Members).Where(up => up.Members.Any(m => m.UserId == userId)));
    }
}
