using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Common;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Requests.Auth;

namespace NXTBackend.API.Controllers;

[Route("search")]
[ApiController]
public class SearchController(ISearchService searchService) : ControllerBase
{
    private readonly ISearchService _searchService = searchService;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        User,
        Project,
        Cursus,
        LearningGoal
    }

    [HttpGet("{category}")]

    //[ProducesResponseType(typeof(IEnumerable<SearchResult>)), 200]
    //[ProducesResponseType(401)]
    //[ProducesResponseType(400)]
    //[ProducesResponseType(500)]
    // public ProducesResponseTypeAttribute(Type type, int statusCode)
    [ProducesResponseType(typeof(SearchResponseDto<User>), 200)] // Can respond like this if Category enum is User
    //[ProducesResponseType(typeof(SearchResponseDto<Cursus>), 200)]
    public async Task<IActionResult> Search(
        Category category,
        [FromQuery] PaginationParams pagination,
        [FromQuery] SearchRequestDto body
    )
    {
        return Ok(category switch
        {
            Category.User => await _searchService.SearchUserAsync(body, pagination),
            Category.Project => await _searchService.SearchProjectAsync(body, pagination),
            Category.Cursus => await _searchService.SearchCursusAsync(body, pagination),
            Category.LearningGoal => await _searchService.SearchGoalAsync(body, pagination),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        });
    }
}
