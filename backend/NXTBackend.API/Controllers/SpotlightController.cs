// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Spotlight;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("events"), ApiController, Authorize]
public class EventController(ISpotlightEventService notificationService) : ControllerBase
{
    /// <summary>
    /// Get all events
    /// </summary>
    /// <response code="200">Ok</response>
    ///
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<SpotlightEvent>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/spotlights")]
    public async Task<IActionResult> GetSpotlights([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <response code="200">Ok</response>
    ///
    /// <response code="403">Forbidden</response>
    /// <response code="409">Conflict</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<SpotlightEvent>(200)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpPost("/spotlights")]
    public async Task<IActionResult> CreateSpotlight([FromBody] SpotlightPostRequestDTO data)
    {
        if (await notificationService.FindByTitleAsync(data.Title) is not null)
            return Conflict("Event already exists");

        return Ok(await notificationService.CreateAsync(new()
        {
            Title = data.Title,
            Description = data.Description,
            Href = data.Href,
            ActionText = data.HrefText,
            BackgroundUrl = data.BackgroundUrl,
        }));
    }

    /// <summary>
    /// Get a specific event
    /// </summary>
    /// <response code="200">Ok</response>
    ///
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<SpotlightEvent>(200)]
    [HttpGet("/events/{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var spotlight = await notificationService.FindByIdAsync(id);
        return spotlight is null ? NotFound("Event not found") : Ok(spotlight);
    }

    /// <summary>
    /// Patch a specific event
    /// </summary>
    /// <response code="200">Ok</response>
    ///
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<SpotlightEvent>(200)]
    [HttpPatch("/events/{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] SpotlightPatchRequestDTO data)
    {
        var @event = await notificationService.FindByIdAsync(id);
        if (@event is null)
            return NotFound(new ErrorResponseDto("Event not found"));

        @event.Title = data.Title ?? @event.Title;
        @event.Description = data.Description ?? @event.Description;
        @event.Href = data.Href ?? @event.Href;
        @event.ActionText = data.HrefText ?? @event.ActionText;
        @event.BackgroundUrl = data.BackgroundUrl ?? @event.BackgroundUrl;

        await notificationService.UpdateAsync(@event);
        return Ok(@event);
    }

    /// <summary>
    /// Delete a specific event
    /// </summary>
    /// <response code="200">Ok</response>
    ///
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<SpotlightEvent>(200)]
    [HttpDelete("/events/{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var @event = await notificationService.FindByIdAsync(id);
        if (@event is null)
            return NotFound(new ErrorResponseDto("Event not found"));

        await notificationService.DeleteAsync(@event);
        return NoContent();
    }
}
