using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Responses.Objects.SearchResponses;

namespace NXTBackend.API.Controllers;

[Route("search")]
[ApiController]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpGet("{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SearchResultDO>>> Search(
        [FromQuery(Name = "filter[category]")] SearchKind? category,
        [FromQuery(Name = "filter[query]")] string query,
        [FromQuery] PaginationParams pagination
    )
    {
        var page = searchService.SearchAsync(query, pagination, category);
        return Ok();
    }
}
