﻿// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Event;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("events"), ApiController, Authorize]
public class EventController(IEventService eventService) : ControllerBase
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
    [ProducesResponseType<IEnumerable<Event>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/events")]
    public async Task<IActionResult> GetEvents([FromQuery] PaginationParams pagination)
    {
        var list = await eventService.GetAllAsync(pagination);
        var headers = list.GetHeaders();
        foreach (var header in headers)
            HttpContext.Response.Headers.Append(header.Key, header.Value);
        return Ok(list.Items);
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
    [ProducesResponseType<Event>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPost("/events")]
    public async Task<IActionResult> CreateEvent([FromBody] EventPostRequestDto request)
    {
        if (await eventService.FindByNameAsync(request.Title) is not null)
            return Conflict(new ErrorResponseDto("Event already exists"));

        var @event = await eventService.CreateAsync(new ()
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
    [ProducesResponseType<Event>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/events/{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var @event = await eventService.FindByIdAsync(id);
        if (@event is null)
            return NotFound(new ErrorResponseDto("Event not found"));
        return Ok(@event);
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
    [ProducesResponseType<Event>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPatch("/events/{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventPatchRequestDto request)
    {
        var @event = await eventService.FindByIdAsync(id);
        if (@event is null)
            return NotFound(new ErrorResponseDto("Event not found"));

        @event.Title = request.Title ?? @event.Title;
        @event.Description = request.Description ?? @event.Description;
        @event.Href = request.Href ?? @event.Href;
        @event.ActionText = request.HrefText ?? @event.ActionText;
        @event.BackgroundUrl = request.BackgroundUrl ?? @event.BackgroundUrl;

        await eventService.UpdateAsync(@event);
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
    [ProducesResponseType<Event>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpDelete("/events/{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var @event = await eventService.FindByIdAsync(id);
        if (@event is null)
            return NotFound(new ErrorResponseDto("Event not found"));

        await eventService.DeleteAsync(@event);
        return NoContent();
    }
}