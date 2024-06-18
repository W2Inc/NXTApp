using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Common;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Requests.Auth;
using NXTBackend.API.Models.Responses.Auth;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User?> FindByIdAsync(Guid id) => await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> FindByLoginAsync(string login) => await _databaseContext.Users.FirstOrDefaultAsync(u => u.Login == login);

    public async Task<User?> FindByNameAsync(string displayName) => await _databaseContext.Users.FirstOrDefaultAsync(u => u.DisplayName == displayName);

    public async Task<User> CreateAsync(User entity)
    {
        var user = await _databaseContext.Users.AddAsync(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public User Delete(User entity)
    {
        var user = _databaseContext.Users.Remove(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public User Update(User entity)
    {
        var user = _databaseContext.Users.Update(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public async Task<PaginatedList<User>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Users.Select(User => User);
        return await PaginatedList<User>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);
    }

    public Task<User> SubscribeToCursus(User entity)
    {
        throw new NotImplementedException();
    }
}
