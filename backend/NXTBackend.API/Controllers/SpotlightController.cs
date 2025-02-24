// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Spotlight;
using NXTBackend.API.Models.Responses.Objects;

// ============================================================================

namespace NXTBackend.API.Controllers;

[ApiController]
[Route("spotlights"), Authorize]
public class SpotlightController(
    ILogger<SpotlightController> logger,
    ISpotlightEventService spotlightService
) : Controller
{
    [HttpGet("/spotlights")]
    [EndpointSummary("Get all spotlights")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SpotlightEventDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[id]")] Guid? id
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("id", id);

        var page = await spotlightService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(item => new SpotlightEventDO(item)));
    }

    [HttpPost("/spotlights"), Authorize(Roles = "events")]
    [EndpointSummary("Create a spotlight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpotlightEventDO>> Create([FromBody] SpotlightPostRequestDTO data)
    {
        var entity = await spotlightService.CreateAsync(new()
        {
            Title = data.Title,
            Description = data.Description,
            ActionText = data.HrefText, // TODO: Same name would be nice
            Href = data.Href,
            BackgroundUrl = data.BackgroundUrl
        });

        return Ok(new SpotlightEventDO(entity));
    }

    [HttpGet("/spotlights/{id:guid}")]
    [EndpointSummary("Get a spotlight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpotlightEventDO>> Get(Guid id)
    {
        var entity = await spotlightService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();
        return Ok(new SpotlightEventDO(entity));
    }

    [HttpPatch("/spotlights/{id:guid}"), Authorize(Roles = "events")]
    [EndpointSummary("Update a spotlight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpotlightEventDO>> Update(Guid id, [FromBody] SpotlightPatchRequestDTO data)
    {
        var entity = await spotlightService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();

        var updatedSpotlight = await spotlightService.UpdateAsync(entity);
        return Ok(new SpotlightEventDO(updatedSpotlight));
    }

    [HttpDelete("/spotlights/{id:guid}"), Authorize(Roles = "events")]
    [EndpointSummary("Delete a spotlight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpotlightEventDO>> Delete(Guid id)
    {
        var entity = await spotlightService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();

        await spotlightService.DeleteAsync(entity);
        return Ok(new SpotlightEventDO(entity));
    }
}
