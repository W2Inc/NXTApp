using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class UserService(DatabaseContext ctx) : BaseService<User>(ctx), IUserService
{
    /// <inheritdoc/>
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
    }

    /// <inheritdoc/>
    public async Task<User?> FindByNameAsync(string displayName)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.DisplayName == displayName);
    }

    /// <inheritdoc/>
    public Task<User> SubscribeToCursus(User entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<Review?> didRecentReviewOnUserProject(User entity, UserProject userProject)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> SubscribeToCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<User> UnsubscribeFromCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<PaginatedList<Notification>> GetNotifications(User entity)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<NotificationAction> DismissNotification(User entity, Notification notification)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override async Task<PaginatedList<User>> GetAllAsync(PaginationParams pagination, SortingParams sorting)
    {
        var query = _dbSet
            .Include(user => user.Details)
            .AsQueryable();

        query = await SortedList<User>.ApplyAsync(query, sorting);
        return await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<User> UpdateDetails(User entity, Details details)
    {
        var detailsEntity = await _context.Details.AddAsync(details);
        entity.DetailsId = detailsEntity.Entity.Id;
        entity.Details = detailsEntity.Entity;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<PaginatedList<UserCursus>> GetUserCursi(User entity, PaginationParams pagination)
    {
        var query = _context.UserCursi
            .Include(c => c.Cursus)
            .Include(c => c.Cursus.Creator)
            .AsQueryable();

        return await PaginatedList<UserCursus>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<PaginatedList<UserGoal>> GetUserGoals(User entity, PaginationParams pagination)
    {
        var query = _context.UserGoals
            .Include(c => c.Members)
            .Include(c => c.User)
            .AsQueryable();

        return await PaginatedList<UserGoal>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<PaginatedList<UserProject>> GetUserProjects(User entity, PaginationParams pagination)
    {
        var query = _context.UserProject
            .Include(c => c.GitInfo)
            .Include(c => c.Project)
            .Include(c => c.Rubric)
            .Include(c => c.Members)
            .AsQueryable();

        return await PaginatedList<UserProject>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public Task<Member> InviteUserToProject(User entity, UserProject instance, MemberInviteState invitation)
    {
        throw new NotImplementedException();
    }
}
