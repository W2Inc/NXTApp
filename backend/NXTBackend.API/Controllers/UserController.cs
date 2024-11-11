using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IUserService userService,
    ISpotlightEventService spotlightService,
    ISpotlightEventActionService spotlightActionService
) : Controller
{
    /// <summary>
    /// Get the currently authenticated user.
    /// </summary>
    /// <returns>The user, aka, you.</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    [ProducesResponseType(typeof(UserDO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/current"), Authorize]
    public async Task<IActionResult> CurrentUserAsync()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        return user is null ? Forbid() : Ok(new UserDO(user));
    }

    /// <summary>
    /// Get the current user's events
    /// </summary>
    /// <returns>Your Events</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    [ProducesResponseType(typeof(IEnumerable<SpotlightEvent>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/current/notifications"), Authorize]
    public async Task<IActionResult> GetNotifications()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        var list = await userService.GetNotifications(user);
        list.AppendHeaders(Response.Headers);
        return Ok(list.Items);
    }

    /// <summary>
    /// Dismiss a notification, making sure it is not shown again to the user.
    /// </summary>
    /// <param name="id">The notifications id to dismiss.</param>
    /// <returns>None</returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    [ProducesResponseType(typeof(SpotlightEventActionDO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete("/users/current/notifications/{id}"), Authorize]
    public async Task<IActionResult> DismissNotification(Guid id)
    {
        if (User.GetSID() != id)
            return Forbid();

        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return Forbid();

        var notification = await spotlightService.FindByIdAsync(id);
        if (notification is null)
            return NotFound("Notification not found");

        var action = await spotlightActionService.Upsert(user, notification, true);
        return Ok(new SpotlightEventActionDO(action));
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="paging">Page parameters</param>
    /// <param name="sorting">Sort parameters</param>
    /// <returns></returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType(typeof(IEnumerable<UserDO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users"), OutputCache]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paging, [FromQuery] SortingParams sorting)
    {
        var page = await userService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserDO(e)));
    }

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">The user to get</param>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="429">Too many requests</response>
    [ProducesResponseType(typeof(IEnumerable<UserDO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        return user is null ? NotFound() : Ok(new UserDO(user));
    }

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType<UserDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpPatch("/users/{id}"), Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserPatchRequestDTO data)
    {
        if (User.GetSID() != id && !User.IsInRole(Role.Admin.ToString()))
            return Forbid();

        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound();

        user.DisplayName = data.DisplayName ?? user.DisplayName;
        user.AvatarUrl = data.AvatarUrl ?? user.AvatarUrl;
        return Ok(new UserDO(user));
    }

    /// <summary>
    /// Update a user's details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType<UserDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpPut("/users/{id}/details"), Authorize]
    public async Task<IActionResult> UpdateUserDetails(Guid id, [FromBody] UserDetailsPutRequestDTO data)
    {
        if (User.GetSID() != id && !User.IsInRole(Role.Admin.ToString()))
            return Forbid();

        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound();

        return Ok(new UserDO(await userService.UpdateDetails(user, new()
        {
            Bio = data.Bio ?? user.Details?.Bio,
            Email = data.Bio ?? user.Details?.Email,
            FirstName = data.Bio ?? user.Details?.FirstName,
            LastName = data.Bio ?? user.Details?.LastName,
            GithubUrl = data.Bio ?? user.Details?.GithubUrl,
            WebsiteUrl = data.Bio ?? user.Details?.WebsiteUrl,
            TwitterUrl = data.Bio ?? user.Details?.TwitterUrl,
            LinkedinUrl = data.Bio ?? user.Details?.LinkedinUrl,
        })));
    }

    /// <summary>
    /// Get all the cursus instances of a user.
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="pagination"></param>
    /// <remarks>
    /// Gets you all the instances of a user's cursus. Essentially "which cursi am I subscribed to ?"
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType<IEnumerable<UserCursusDO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/{id}/user_cursus")]
    public async Task<IActionResult> GetUserCursus(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        var cursi = await userService.GetUserCursi(user, pagination);
        return Ok(cursi.Items.Select(c => new UserCursusDO(c)));
    }

    /// <summary>
    /// Get all the goal instances of a user.
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="pagination"></param>
    /// <remarks>
    /// Gets you all the instances of a user's cursus. Essentially "which cursi am I subscribed to ?"
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType<IEnumerable<UserGoalDO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/{id}/user_goals")]
    public async Task<IActionResult> GetUserGoals(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        var cursi = await userService.GetUserGoals(user, pagination);
        return Ok(cursi.Items.Select(c => new UserGoalDO(c)));
    }

    /// <summary>
    /// Get all the goal instances of a user.
    /// </summary>
    /// <param name="id">The user id</param>
    /// <param name="pagination"></param>
    /// <remarks>
    /// Gets you all the instances of a user's cursus. Essentially "which cursi am I subscribed to ?"
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    [ProducesResponseType<IEnumerable<UserProjectDO>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [HttpGet("/users/{id}/user_projects")]
    public async Task<IActionResult> GetUserProjects(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        var cursi = await userService.GetUserProjects(user, pagination);
        return Ok(cursi.Items.Select(c => new UserProjectDO(c)));
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <remarks>

    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="500">Internal Error</response>
    // [ProducesResponseType<IEnumerable<UserCursusDO>>(StatusCodes.Status200OK)]
    // [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    // [HttpPost("/user/{id}/user_projects/{user_project_id}/invite/accept")] // Accept
    // public async Task<IActionResult> ()
    // {

    // }
}
