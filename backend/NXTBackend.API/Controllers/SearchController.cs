using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Filters;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses;
using NXTBackend.API.Models.Responses.Objects;

namespace NXTBackend.API.Controllers;

[Route("search")]
[ApiController]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpGet("{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> Search(
        Category category,
        [FromQuery] PaginationParams pagination,
        [FromQuery] SearchRequestDTO body
    )
    {
        var results = category switch
        {
            Category.User => await SearchUsers(body, pagination),
            Category.Project => await SearchProjects(body, pagination),
            Category.Cursus => await SearchCursus(body, pagination),
            Category.LearningGoal => await SearchLearningGoals(body, pagination),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

        return Ok(results);
    }

    private async Task<IEnumerable<object>> SearchUsers(SearchRequestDTO body, PaginationParams pagination)
    {
        var users = await searchService.SearchAsync<User>(
            body, pagination,
            dbSet => dbSet
                .Where(x => EF.Functions.Like(x.Login, $"%{body.Query}%"))
                .Include(x => x.Details)
        );
        return users.Select(user => new UserDO(user));
    }

    private async Task<IEnumerable<object>> SearchProjects(SearchRequestDTO body, PaginationParams pagination)
    {
        var projects = await searchService.SearchAsync<Project>(
            body, pagination,
            dbSet => dbSet
                .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
                .Include(x => x.GitInfo)
        );
        return projects.Select(project => new ProjectDO(project));
    }

    private async Task<IEnumerable<object>> SearchCursus(SearchRequestDTO body, PaginationParams pagination)
    {
        var cursus = await searchService.SearchAsync<Cursus>(
            body, pagination,
            dbSet => dbSet
                .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
                .Include(x => x.Creator)
        );
        return cursus.Select(cursusItem => new CursusDO(cursusItem));
    }

    private async Task<IEnumerable<object>> SearchLearningGoals(SearchRequestDTO body, PaginationParams pagination)
    {
        var learningGoals = await searchService.SearchAsync<LearningGoal>(
            body, pagination,
            dbSet => dbSet
                .Where(x => EF.Functions.Like(x.Name, $"%{body.Query}%"))
                .Include(x => x.Creator)
        );
        return learningGoals.Select(learningGoal => new LearningGoalDO(learningGoal));
    }
}
