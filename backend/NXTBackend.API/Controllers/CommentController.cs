﻿// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Event;
using NXTBackend.API.Models.Requests.Feature;
using NXTBackend.API.Models.Requests.Review;
using NXTBackend.API.Models.Responses;
using Serilog;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("comments")]
[ApiController]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : ControllerBase
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
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/comments")]
    public async Task<IActionResult> GetComments([FromQuery] PaginationParams pagination)
    {
        var list = await commentService.GetAllAsync(pagination);
        var headers = list.GetHeaders();
        foreach (var header in headers)
            HttpContext.Response.Headers.Append(header.Key, header.Value);
        return Ok(list.Items);
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
    [ProducesResponseType<Event>(200)]
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
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(401)]
    [ProducesResponseType<ErrorResponseDto>(403)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/comments/{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        var eventData = await commentService.FindByIdAsync(id);
        if (eventData is null)
            return NotFound(new ErrorResponseDto("Comment not found"));
        return Ok(eventData);
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
    [ProducesResponseType<Event>(200)]
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
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(401)]
    [ProducesResponseType<ErrorResponseDto>(403)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(429)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpDelete("/comments/{id}"), Authorize]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var comment = await commentService.FindByIdAsync(id);
        if (comment is null)
            return NotFound(new ErrorResponseDto("Event not found"));
        return Ok(await commentService.DeleteAsync(comment));
    }
}
