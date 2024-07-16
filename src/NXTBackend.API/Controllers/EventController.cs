// ============================================================================
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
    /// <returns>All current existing events</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/events"), Authorize]
    public async Task<IActionResult> GetEvents(
        [FromQuery] PaginationParams pagination
    )
    {
        try
        {
            var list = await eventService.GetAllAsync(pagination);
            HttpContext.Response.Headers.Append("X-Page", list.Page.ToString());
            HttpContext.Response.Headers.Append("X-Next-Page", list.HasNextPage.ToString());
            HttpContext.Response.Headers.Append("X-Prev-Page", list.HasPreviousPage.ToString());
            HttpContext.Response.Headers.Append("X-Count", list.TotalCount.ToString());
            HttpContext.Response.Headers.Append("X-Pages", list.TotalPages.ToString());
            // Add every user claim

            // Log debug all the user claims
            //var claims = HttpContext.User.Claims;
            //var ids = HttpContext.User.Identities.ToList();
            //foreach (var claim in claims)
            //{
            //    Log.Information($"Claim: {claim.Type}, Value: {claim.Value}");
            //}

            //foreach (var identity in ids)
            //{
            //    Log.Information($"Identity: {identity.Name}");
            //}

            //// get the user id
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Log.Information($"User ID: {userId}");

            return Ok(list.Items);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while fetching events");
            return StatusCode(500, new ErrorResponseDto());
        }
    }

    /// <summary>
    /// Get an Event by its id
    /// </summary>
    /// <returns> The updated feature</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/events/{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var eventData = await eventService.FindByIdAsync(id);
        if (eventData is null)
            return NotFound(new ErrorResponseDto("Event not found"));
        return Ok(eventData);
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <returns> Returns the new event</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(409)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpPost("/events"), Authorize("dev")]
    public async Task<IActionResult> SetEvent([FromBody] EventPostRequestDto body)
    {
        if (await eventService.FindByNameAsync(body.Title) is not null)
            return Conflict(new ErrorResponseDto("An event with that title already exists"));

        return Ok(await eventService.CreateAsync(new()
        {
            Title = body.Title,
            Description = body.Description,
            Href = body.Href,
            ActionText = body.HrefText,
            BackgroundUrl = body.BackgroundUrl
        }));
    }


    /// <summary>
    /// Update an event
    /// </summary>
    /// <returns>The updated event</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="422">Unprocessable Entity</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(422)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpPatch("/events/{id}"), Authorize("dev")]
    public async Task<IActionResult> SetEvent(Guid id, [FromBody] EventPatchRequestDto body)
    {
        if (body.Title is not null && await eventService.FindByNameAsync(body.Title) is not null)
            return Conflict(new ErrorResponseDto("An event with that title already exists"));

        var eventData = await eventService.FindByIdAsync(id);
        if (eventData is null)
            return NotFound(new ErrorResponseDto("Event not found"));

        return Ok(await eventService.UpdateAsync(new()
        {
            Id = id,
            Title = body.Title ?? eventData.Title,
            Description = body.Description ?? eventData.Description,
            Href = body.Href ?? eventData.Href,
            ActionText = body.HrefText ?? eventData.ActionText,
            BackgroundUrl = body.BackgroundUrl ?? eventData.BackgroundUrl
        }));
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    /// <returns> </returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpDelete("/events/{id}"), Authorize("dev")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var eventData = await eventService.FindByIdAsync(id);
        if (eventData is null)
            return NotFound(new ErrorResponseDto("Event not found"));
        return Ok(await eventService.DeleteAsync(eventData));
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    /// <returns> </returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    //[ProducesResponseType<Event>(200)]
    //[ProducesResponseType<ErrorResponseDto>(400)]
    //[ProducesResponseType<ErrorResponseDto>(500)]
    //[HttpDelete("/events/{id}"), Authorize("dev")]
    //public async Task<IActionResult> DeleteEvent(Guid id)
    //{
    //    var eventData = await eventService.FindByIdAsync(id);
    //    if (eventData is null)
    //        return NotFound(new ErrorResponseDto("Event not found"));
    //    return Ok(await eventService.DeleteAsync(eventData));
    //}
}
