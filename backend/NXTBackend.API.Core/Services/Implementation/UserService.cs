using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Models.Responses.Objects;
using Microsoft.AspNetCore.Http;

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
    public override async Task<PaginatedList<User>> GetAllAsync(PaginationParams pagination, SortingParams sorting, FilterDictionary? filters = null)
    {
        var query = _dbSet
            .Include(user => user.Details)
            .AsQueryable();

        query = ApplyFilters(query, filters);
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

    /// <inheritdoc/>
    public async Task<SpotlightEventAction> SetSpotlight(Guid userId, Guid spotlightId, bool action)
    {
        // Verify the spotlight event exists
        if (!await _context.SpotlightEvents.AnyAsync(se => se.Id == spotlightId))
            throw new ServiceException("The specified spotlight event does not exist.");

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

    public async Task<UserProject> SubscribeToProject(Guid userId, Guid projectId)
    {
        // First try to find any inactive project this user might have an activate it again.
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId) ?? throw new ServiceException("Project not found");
        var user = await FindByIdAsync(userId) ?? throw new ServiceException("User not found");
        var exisitingUserProject = await _context
            .UserProject
            .Where(up => up.Members.Any(m => m.UserId == userId) && up.ProjectId == projectId)
            .FirstOrDefaultAsync();

        // TODO: Execute custom rules!

        if (exisitingUserProject is null)
        {
            var userProject = await _context.UserProject.AddAsync(new()
            {
                State = TaskState.Active,
                ProjectId = projectId,
                GitInfoId = null,
                Members = []
            });

            await _context.Members.AddAsync(new()
            {
                UserId = userId,
                State = MemberInviteState.Accepted,
                UserProjectId = userProject.Entity.Id
            });

            await _context.SaveChangesAsync();
            return userProject.Entity;
        }

        switch (exisitingUserProject.State)
        {
            case TaskState.Completed:
                throw new ServiceException("Cannot Re-subscribe to a completed project");
            case TaskState.Active:
            case TaskState.Awaiting:
                throw new ServiceException("User is already subscribed");
            case TaskState.Inactive:
                exisitingUserProject.State = TaskState.Active;
                var userProject = _context.UserProject.Update(exisitingUserProject);
                await _context.SaveChangesAsync();
                return userProject.Entity;
            default:
                throw new NotImplementedException("TaskState not being handled");
        }
    }

    public async Task<UserProject> UnsubscribeFromProject(Guid userId, Guid projectId)
    {
        // 1. State must only be active
        // 2. Must not be the last user to leave
        // 3. If last user and no git info or reviews on user project then hard delete
        // 4. Switch state to Inactive instead

        var userProject = await _context
            .UserProject
            .Include(up => up.Project)
            .Include(up => up.GitInfo)
            .Where(up => up.Members.Any(m => m.UserId == userId) && up.ProjectId == projectId)
            .FirstOrDefaultAsync();

        if (userProject is null)
            throw new ServiceException("User project does not exist");
        if (userProject.State != TaskState.Active)
        {
            string detail = $"The project can't be unsubscribed from when in the following state: {userProject.State}";
            throw new ServiceException(StatusCodes.Status422UnprocessableEntity, "Unable to unsubscribe", detail);
        }

        bool isLastMember = userProject.Members.Count == 1;
        bool hasReviews = _context
            .Reviews
            .Where(r => r.UserProjectId == userProject.Id)
            .Any();

        if (isLastMember && !hasReviews)
        {
            _context.UserProject.Remove(userProject);
            _context.Members.RemoveRange(userProject.Members);
        }
        else
        {
            userProject.State = TaskState.Inactive;
            _context.UserProject.Update(userProject);
        }

        await _context.SaveChangesAsync();
        return userProject;
    }

    public async Task<User?> UpsertDetails(Guid id, Details details)
    {
        const int C_MAX_BIO = 16384;
        if (details.Bio is not null && details.Bio.Length > C_MAX_BIO)
            throw new ServiceException($"Biographies can only be {C_MAX_BIO} characters long");

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
