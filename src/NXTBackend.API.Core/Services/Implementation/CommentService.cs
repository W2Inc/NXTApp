using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class CommentService : ICommentService
{
    private readonly DatabaseContext _databaseContext;

    public CommentService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<Comment> CreateAsync(Comment entity)
    {
        var comment = _databaseContext.Comments.Add(entity);
        await _databaseContext.SaveChangesAsync();
        return comment.Entity;
    }

    /// <inheritdoc/>
    public async Task<Comment> DeleteAsync(Comment entity)
    {
        var comment = _databaseContext.Comments.Remove(entity);
        await _databaseContext.SaveChangesAsync();
        return comment.Entity;
    }

    /// <inheritdoc/>
    public async Task<Comment?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.Comments.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<PaginatedList<Comment>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Comments.Select(Comment => Comment);
        return PaginatedList<Comment>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    /// <inheritdoc/>
    public async Task<Comment> UpdateAsync(Comment entity)
    {
        var comment = _databaseContext.Comments.Update(entity).Entity;
        await _databaseContext.SaveChangesAsync();
        return comment;
    }
}
