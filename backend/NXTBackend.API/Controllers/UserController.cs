using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IUserService userService,
    INotificationService eventService
) : ControllerBase
{
    /// <summary>
    /// Get the currently authenticated user.
    /// </summary>
    /// <returns>The user, aka, you.</returns>
    /// <param name=""></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1"
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(typeof(UserDO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/current"), Authorize]
    public async Task<IActionResult> CurrentUser()
    {
        var user = HttpContext.GetUser();
        return user is null ? Forbid() : Ok(new UserDO(user));
    }

    /// <summary>
    /// Get the current user's events
    /// </summary>
    /// <param name=""></param>
    /// <returns>Your Events</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1"
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/current/notifications")]
    public async Task<IActionResult> GetCurrentEvents()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Dismiss a notification, making sure it is not shown again to the user.
    /// </summary>
    /// <param name="id">The notifiction id to dismiss.</param>
    /// <returns>None</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("/users/current/notifications/{id}")]
    public async Task<IActionResult> DismissNotification(Guid Id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="paging">Page parameters</param>
    /// <param name="sorting">Sort parameters</param>
    /// <returns></returns>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(typeof(IEnumerable<UserDO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users"), Authorize]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paging, [FromQuery] SortingParams sorting)
    {
        var user = HttpContext.GetUser();
        logger.Log(LogLevel.Information, "HELLO WORLD!, {@user}", user);
        if (user is null)
        {
            logger.LogInformation("NO USER!");
        }

        var page = await userService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserDO(e)));
    }
}
