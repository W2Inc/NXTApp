// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;

// ============================================================================

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
    // Current
    // ============================================================================


    [HttpGet("/users/current"), Authorize]
    [EndpointSummary("Get the currently authenticated user.")]
    [EndpointDescription("When authenticated it's useful to know who you currently are logged in as.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDO>> CurrentUserAsync()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        return user is null ? Forbid() : Ok(new UserDO(user));
    }

    [HttpGet("/users/current/spotlights"), Authorize, OutputCache(Duration = 20)]
    [EndpointSummary("Get the currently authenticated user's spotlights.")]
    [EndpointDescription("If they were dismissed they will get filtered out.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpotlightEvent>>> GetSpotlights()
    {
        return Ok(await userService.GetSpotlights(User.GetSID()));
    }

    [HttpDelete("/users/current/spotlights/{id}"), Authorize]
    [EndpointSummary("As the current user, dismiss a specific spotlight.")]
    [EndpointDescription("If users dismiss a spotlight event, they won't getit shown in the future.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<SpotlightEventActionDO>> DismissSpotlight(Guid id)
    {
        try
        {
            var action = await userService.SetSpotlight(User.GetSID(), id, true);
            return Ok(new SpotlightEventActionDO(action));
        }
        catch (ArgumentException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }

    [HttpGet("/users/current/notifications"), Authorize]
    [EndpointSummary("Get your notifications")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        return Problem(statusCode: StatusCodes.Status501NotImplemented);
    }

    [HttpPost("/users/current/notifications/read"), Authorize]
    [EndpointSummary("Mark all your notifications as read")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        return Problem(statusCode: StatusCodes.Status501NotImplemented);
    }

    [HttpGet("/users/current/notifications/{id}/read"), Authorize]
    [EndpointSummary("Mark a given notifications as read")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkNotificationAsRead()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        return Problem(statusCode: StatusCodes.Status501NotImplemented);
    }


    [HttpGet("/users/current/events"), Authorize]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        return Problem(statusCode: StatusCodes.Status501NotImplemented);
    }

    // Users
    // ============================================================================

    [HttpGet("/users")]
    [EndpointSummary("Get all users.")]
    [EndpointDescription("All users that exists in the database.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDO>>> GetAll([FromQuery] PaginationParams paging, [FromQuery] SortingParams sorting)
    {
        var page = await userService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserDO(e)));
    }

    [HttpGet("/users/{id}")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDO>> GetUser(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        return user is null ? NotFound() : Ok(new UserDO(user));
    }

    // [ProducesResponseType<UserDO>(StatusCodes.Status200OK)]
    [HttpPatch("/users/{id}"), Authorize]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDO>> UpdateUser(Guid id, [FromBody] UserPatchRequestDTO data)
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

    [HttpPut("/users/{id}/details"), Authorize]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDO>> UpdateUserDetails(Guid id, [FromBody] UserDetailsPutRequestDTO data)
    {
        var userID = User.GetSID();
        if (userID != id && !User.IsInRole(Role.Admin.ToString()))
            return Forbid();

        var updatedUser = await userService.UpsertDetails(userID, new()
        {
            Bio = data.Bio,
            Email = data.Email,
            FirstName = data.FirstName,
            LastName = data.LastName,
            GithubUrl = data.GithubUrl,
            WebsiteUrl = data.WebsiteUrl,
            TwitterUrl = data.TwitterUrl,
            LinkedinUrl = data.LinkedinUrl,
        });

        if (updatedUser is null)
            return NotFound("User not found");

        return Ok(new UserDO(updatedUser));
    }

    [HttpGet("/users/{id}/user_cursus")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserCursusDO[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserCursus(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        return Ok();
        // var cursi = await userService.GetUserCursi(user, pagination);
        // return Ok(cursi.Items.Select(c => new UserCursusDO(c)));
    }

    [HttpGet("/users/{id}/user_goals")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserGoalDO[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserGoals(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        return Ok();
        // var cursi = await userService.GetUserGoals(user, pagination);
        // return Ok(cursi.Items.Select(c => new UserGoalDO(c)));
    }


    [HttpGet("/users/{id}/user_projects")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProjects(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        return Ok();
        // var cursi = await userService.GetUserProjects(user, pagination);
        // return Ok(cursi.Items.Select(c => new UserProjectDO(c)));
    }
}
