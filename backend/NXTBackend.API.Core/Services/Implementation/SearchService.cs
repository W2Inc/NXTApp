using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class SearchService : ISearchService
{
    private readonly DatabaseContext _databaseContext;

    public SearchService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<SearchResponseDto<Cursus>> SearchCursusAsync(SearchRequestDto request, PaginationParams pagination)
    {
        var query = _databaseContext.Cursi.Where(x => EF.Functions.Like(x.Name, $"%{request.Query}%"));
        var data = await PaginatedList<Cursus>.CreateAsync(query, pagination.Page, pagination.Size);

        return new()
        {
            Results = data.Items
        };
    }

    async Task<SearchResponseDto<LearningGoal>> ISearchService.SearchGoalAsync(SearchRequestDto request, PaginationParams pagination)
    {
        var query = _databaseContext.LearningGoals.Where(x => EF.Functions.Like(x.Name, $"%{request.Query}%"));
        var data = await PaginatedList<LearningGoal>.CreateAsync(query, pagination.Page, pagination.Size);

        return new()
        {
            Results = data.Items
        };
    }

    async Task<SearchResponseDto<Project>> ISearchService.SearchProjectAsync(SearchRequestDto request, PaginationParams pagination)
    {
        var query = _databaseContext.Projects.Where(x => EF.Functions.Like(x.Name, $"%{request.Query}%"));
        var data = await PaginatedList<Project>.CreateAsync(query, pagination.Page, pagination.Size);

        return new()
        {
            Results = data.Items
        };
    }

    async Task<SearchResponseDto<User>> ISearchService.SearchUserAsync(SearchRequestDto request, PaginationParams pagination)
    {
        var query = _databaseContext.Users
            .Where(x => EF.Functions.Like(x.Login, $"%{request.Query.ToLower()}%"));
        var data = await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);

        return new()
        {
            Results = data.Items
        };
    }
}
