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
public sealed class UserGoalService : BaseService<UserGoal>, IUserGoalService
{
    public UserGoalService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("user_id", (q, id) => q.Where(ug => ug.UserId == id));
        DefineFilter<string>("name", (q, name) => q.Where(ug => EF.Functions.Like(ug.Goal.Name, $"%{name}%")));
    }
}
