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
    public override async Task<PaginatedList<User>> GetAllAsync(PaginationParams pagination, SortingParams sorting)
    {
        var query = _dbSet
            .Include(user => user.Details)
            .AsQueryable();

        query = await SortedList<User>.ApplyAsync(query, sorting);
        return await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public async Task<IEnumerable<SpotlightEvent>> GetSpotlights(Guid id)
    {
        var spotlights = await _context.SpotlightEvents
            .Where(se => !_context.SpotlightEventsActions
                .Any(sa => sa.UserId == id && sa.IsDismissed && sa.SpotlightId == se.Id))
            .ToListAsync();

        return spotlights;
    }


    public async Task<SpotlightEventAction> SetSpotlight(Guid userId, Guid spotlightId, bool action)
    {
        // Verify the spotlight event exists
        if (!await _context.SpotlightEvents.AnyAsync(se => se.Id == spotlightId))
            throw new ArgumentException("The specified spotlight event does not exist.");

        // Retrieve or create the SpotlightEventAction entry
        var spotlightAction = await _context.SpotlightEventsActions
            .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.SpotlightId == spotlightId);
        if (spotlightAction is not null)
        {
            spotlightAction.IsDismissed = action;
            await _context.SaveChangesAsync();
            return spotlightAction;
        }

        var actionEvent = await _context.SpotlightEventsActions.AddAsync(new()
        {
            UserId = userId,
            SpotlightId = spotlightId,
            IsDismissed = action,
        });
        await _context.SaveChangesAsync();
        return actionEvent.Entity;
    }


    public async Task<User?> UpsertDetails(Guid id, Details details)
    {
        var user = await _context.Users
            .Include(u => u.Details)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return null;
        if (user.Details is null || user.DetailsId is null)
        {
            // NOTE(W2): A bit finicky, details can't exist without
            // a user and vice verse.
            details.UserId = id;
            await _context.Details.AddAsync(details);
            user.Details = details;
        }
        else
        {
            user.Details.Bio = details.Bio;
            user.Details.Email = details.Email;
            user.Details.FirstName = details.FirstName;
            user.Details.LastName = details.LastName;
            user.Details.GithubUrl = details.GithubUrl;
            user.Details.WebsiteUrl = details.WebsiteUrl;
            user.Details.TwitterUrl = details.TwitterUrl;
            user.Details.LinkedinUrl = details.LinkedinUrl;
        }

        await _context.SaveChangesAsync();
        return user;
    }


}
