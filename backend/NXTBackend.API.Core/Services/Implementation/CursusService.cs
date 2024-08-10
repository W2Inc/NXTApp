using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService : ICursusService
{
    private readonly DatabaseContext _databaseContext;

    public CursusService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<Cursus?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.Cursi.FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Cursus> CreateAsync(Cursus entity)
    {
        var goal = await _databaseContext.Cursi.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    /// <inheritdoc />
    public async Task<Cursus> UpdateAsync(Cursus entity)
    {
        var goal = _databaseContext.Cursi.Update(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    /// <inheritdoc />
    public async Task<Cursus> DeleteAsync(Cursus entity)
    {
        var goal = _databaseContext.Cursi.Remove(entity);
        await _databaseContext.SaveChangesAsync();
        return goal.Entity;
    }

    public Task<PaginatedList<Cursus>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Cursi.Select(c => c);
        return PaginatedList<Cursus>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<Cursus?> FindByNameAsync(string name)
    {
        return await _databaseContext.Cursi.FirstOrDefaultAsync(c => c.Name == name);
    }
}