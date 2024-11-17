using Microsoft.AspNetCore.Http;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService(DatabaseContext ctx) : BaseService<Cursus>(ctx), ICursusService
{
    public Task<Cursus?> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Depending on the state of the cursus either does a "soft delete" or
    /// a hard delete.
    /// </summary>
    /// <remarks>
    /// If a cursus has no references to any goals, projects etc it can be
    /// safely hard deleted. However in most cases this would lead to a
    /// very messed up state of models referencing a cursus that is gone.
    ///
    /// Furthermore for curriculums, a user might want to see their old cursus
    /// even if it is considered "old and stale".
    /// </remarks>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override async Task<Cursus> DeleteAsync(Cursus entity)
    {
        // If no references exist, we can't hard delete.
        entity.Enabled = false;
        if (entity.UserCursi.Count < 1)
        {
            _dbSet.Remove(entity);
        }
        else
        {
            await UpdateAsync(entity);
        }
        await _context.SaveChangesAsync();
        return entity;
    }
}
