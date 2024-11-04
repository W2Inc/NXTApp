using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Responses.Objects;

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IUserService userService,
    INotificationService notificationService
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
    public IActionResult CurrentUser()
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
    [HttpGet("/users/current/notifications"), Authorize]
    public async Task<IActionResult> GetNotifications()
    {
        var user = HttpContext.GetUser();
        if (user is null)
            return Forbid();

        var list = await userService.GetNotifications(user);
        list.AppendHeaders(Response.Headers);
        return Ok(list.Items);
    }

    /// <summary>
    /// Dismiss a notification, making sure it is not shown again to the user.
    /// </summary>
    /// <param name="Id">The notifiction id to dismiss.</param>
    /// <returns>None</returns>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(typeof(NotificationActionDO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("/users/current/notifications/{id}")]
    public async Task<IActionResult> DismissNotification(Guid Id)
    {
        var user = HttpContext.GetUser();
        if (user is null)
            return Forbid();

        var notification = await notificationService.FindByIdAsync(Id);
        if (notification is null)
            return NotFound("Notification not found");

        var action =  await userService.DismissNotification(user, notification);
        return Ok(new NotificationActionDO(action));
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
    [HttpGet("/users")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paging, [FromQuery] SortingParams sorting)
    {
        var page = await userService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserDO(e)));
    }
}
