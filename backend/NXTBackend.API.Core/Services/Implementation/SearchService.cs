using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses;
using NXTBackend.API.Models.Responses.Objects.SearchResponses;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class SearchService(DatabaseContext ctx) : ISearchService
{
    public Task<IEnumerable<SearchResultDO>> SearchAsync(string query, PaginationParams pagination, SearchKind? category = null)
    {
        throw new NotImplementedException();
    }

    //     private async Task<IEnumerable<object>> SearchUsers(SearchRequestDTO body, PaginationParams pagination)
    // {
    //     var users = await searchService.SearchAsync<User>(
    //         body, pagination,
    //         dbSet => dbSet
    //             .Where(x => EF.Functions.Like(x.Login, $"%{body.Query}%"))
    //             .Include(x => x.Details)
    //     );
    //     return users.Select(user => new UserDO(user));
    // }

    // private async Task<IEnumerable<ProjectSearchResultDO>> SearchProjects(SearchRequestDTO body, PaginationParams pagination)
    // {
    //     var projects = await SearchAsync<Project>(
    //         body, pagination,
    //         dbSet => dbSet
    //             .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
    //             .Include(x => x.GitInfo)
    //     );
    //     return projects.Select(project => new ProjectSearchResultDO(project));
    // }

    // private async Task<IEnumerable<object>> SearchCursus(SearchRequestDTO body, PaginationParams pagination)
    // {
    //     var cursus = await searchService.SearchAsync<Cursus>(
    //         body, pagination,
    //         dbSet => dbSet
    //             .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
    //             .Include(x => x.Creator)
    //     );
    //     return cursus.Select(cursusItem => new CursusDO(cursusItem));
    // }

    // private async Task<IEnumerable<object>> SearchLearningGoals(SearchRequestDTO body, PaginationParams pagination)
    // {
    //     var learningGoals = await searchService.SearchAsync<LearningGoal>(
    //         body, pagination,
    //         dbSet => dbSet
    //             .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
    //             .Include(x => x.Creator)
    //     );
    //     return learningGoals.Select(learningGoal => new LearningGoalDO(learningGoal));
    // }
}
