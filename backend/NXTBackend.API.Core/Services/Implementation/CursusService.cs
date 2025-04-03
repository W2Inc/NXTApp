using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService(DatabaseContext ctx) : BaseService<Cursus>(ctx), ICursusService
{
    /// <inheritdoc />
    public async Task<Cursus?> FindByNameAsync(string name)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => EF.Functions.Like(x.Name, $"%{name}%"))
            .FirstOrDefaultAsync();
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
            _dbSet.Remove(entity);
        else
            await UpdateAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<(Cursus?, User?)> FindAsCollaboratorOrOwner(Guid entityId, Guid userId)
    {
        var query = from c in _dbSet.AsNoTracking()
                    where c.Id == entityId
                    select new
                    {
                        Cursus = c,
                        Collaborator = c.Collaborators.FirstOrDefault(co => co.Id == userId)
                    };

        var result = await query.FirstOrDefaultAsync();
        return (result?.Cursus, result?.Collaborator);
    }

    public async Task<bool> AddCollaborator(Guid cursusId, Guid userId)
    {
        var cursus = await _dbSet.Include(c => c.Collaborators)
                                .FirstOrDefaultAsync(c => c.Id == cursusId);
        if (cursus is null)
            return false;

        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            return false;

        if (!cursus.Collaborators.Any(c => c.Id == userId))
            cursus.Collaborators.Add(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCollaborator(Guid cursusId, Guid userId)
    {
        var cursus = await _dbSet.Include(c => c.Collaborators)
                                .FirstOrDefaultAsync(c => c.Id == cursusId);
        if (cursus is null)
            return false;

        var user = cursus.Collaborators.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return false;

        cursus.Collaborators.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
