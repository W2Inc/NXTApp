using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class RubricService : BaseService<Rubric>, IRubricService
{
    public RubricService(DatabaseContext ctx) : base(ctx)
    {
        DefineFilter<Guid>("project_id", (q, id) => q.Where((r) => r.ProjectId == id));
        DefineFilter<string>("name", (q, name) => q.Where((r) => EF.Functions.Like(r.Name, $"%{name}%")));
    }
}
