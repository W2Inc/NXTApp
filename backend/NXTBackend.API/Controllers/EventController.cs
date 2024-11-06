// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Event;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("events"), ApiController, Authorize]
public class EventController(INotificationService notificationService) : ControllerBase
{
    /// <summary>
    /// Get all events
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<Notification>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/events")]
    public async Task<IActionResult> GetEvents([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Conflict</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPost("/events")]
    public async Task<IActionResult> CreateEvent([FromBody] EventPostRequestDto request)
    {
        if (await notificationService.FindByTitleAsync(request.Title) is not null)
            return Conflict(new ErrorResponseDto("Event already exists"));

        var @event = await notificationService.CreateAsync(new()
        {
            Title = request.Title,
            Description = request.Description,
            Href = request.Href,
            ActionText = request.HrefText,
            BackgroundUrl = request.BackgroundUrl,
        });

        return Ok(@event);
    }

    /// <summary>
    /// Get a specific event
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/events/{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var @event = await notificationService.FindByIdAsync(id);
        return @event is null ? NotFound(new ErrorResponseDto("Event not found")) : Ok(@event);
    }

    /// <summary>
    /// Patch a specific event
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPatch("/events/{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventPatchRequestDto data)
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
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
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
