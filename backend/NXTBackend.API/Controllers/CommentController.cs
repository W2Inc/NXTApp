// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Review;

// ============================================================================

namespace NXTBackend.API.Controllers;

public class GetAllParams
{
    [FromQuery(Name = "filter[demo]")]
    public string? NoteFromQueryString { get; set; }
}

[Route("comments")]
[ApiController, Produces("application/json")]
public class CommentController(ILogger<CommentController> logger) : ControllerBase
{
    /// <summary>
    /// Get all comments
    /// </summary>
    /// <returns>All current existing events</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/comments")]
    public async Task<Notification> GetComments([FromQuery] PaginationParams pagination, GetAllParams urlParams)
    {
        // var list = await commentService.GetAllAsync(pagination);
        // var headers = list.GetHeaders();
        // foreach (var header in headers)
        //     HttpContext.Response.Headers.Append(header.Key, header.Value);
        return new Notification();
    }

    /// <summary>
    /// Get all comments
    /// </summary>
    /// <returns>All current existing events</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpPost("/comments")]
    public async Task<IActionResult> AddComment([FromBody] CommentPostRequestDto body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a comment by id
    /// </summary>
    /// <returns> The updated feature</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesResponseType<ErrorResponseDto>(401)]
    [ProducesResponseType<ErrorResponseDto>(403)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/comments/{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        throw new NotImplementedException();

        //var eventData = await commentService.FindByIdAsync(id);
        //if (eventData is null)
        //    return NotFound(new ErrorResponseDto("Comment not found"));
        //return Ok(eventData);
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <returns> Returns the new event</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="422">Unprocessable Entity</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Notification>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(401)]
    [ProducesResponseType<ErrorResponseDto>(403)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(409)]
    [ProducesResponseType<ErrorResponseDto>(429)]
    [ProducesResponseType<ErrorResponseDto>(422)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpPatch("/comments/{id}")]
    public async Task<IActionResult> UpdateComment([FromBody] CommentPatchRequestDto body)
    {
        throw new NotImplementedException();
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
    [ProducesResponseType<Notification>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(401)]
    [ProducesResponseType<ErrorResponseDto>(403)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(429)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpDelete("/comments/{id}"), Authorize]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        throw new NotImplementedException();

        //var comment = await commentService.FindByIdAsync(id);
        //if (comment is null)
        //    return NotFound(new ErrorResponseDto("Event not found"));
        //return Ok(await commentService.DeleteAsync(comment));
    }
}
