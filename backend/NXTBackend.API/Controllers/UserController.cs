﻿// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController, Authorize]
public class UserController(
    ILogger<UserController> logger,
    IUserService userService,
    IUserProjectService userProjectService,
    ISpotlightEventService spotlightService,
    ISpotlightEventActionService spotlightActionService
) : Controller
{
    // Current
    // ============================================================================


    [HttpGet("/users/current")]
    [EndpointSummary("Get the currently authenticated user.")]
    [EndpointDescription("When authenticated it's useful to know who you currently are logged in as.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDO>> CurrentUserAsync()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        return user is null ? Forbid() : Ok(new UserDO(user));
    }

    [HttpGet("/users/current/spotlights")]
    [EndpointSummary("Get the currently authenticated user's spotlights.")]
    [EndpointDescription("If they were dismissed they will get filtered out.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpotlightEvent>>> GetSpotlights()
    {
        return Ok(await userService.GetSpotlights(User.GetSID()));
    }

    [HttpDelete("/users/current/spotlights/{id:guid}")]
    [EndpointSummary("As the current user, dismiss a specific spotlight.")]
    [EndpointDescription("If users dismiss a spotlight event, they won't getit shown in the future.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpotlightEventActionDO>> DismissSpotlight(Guid id)
    {
        var action = await userService.SetSpotlight(User.GetSID(), id, true);
        return Ok(new SpotlightEventActionDO(action));
    }

    [HttpGet("/users/current/notifications")]
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

    [HttpPost("/users/current/notifications/read")]
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

    [HttpGet("/users/current/notifications/{id:guid}/read")]
    [EndpointSummary("Mark a given notifications as read")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkNotificationAsRead()
    {
        var user = await userService.FindByIdAsync(User.GetSID());
        if (user is null)
            return Forbid();

        return Problem(statusCode: StatusCodes.Status501NotImplemented);
    }


    [HttpGet("/users/current/events")]
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

    [EndpointSummary("Get all users.")]
    [EndpointDescription("All users that exists in the database.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("/users"), OutputCache(PolicyName = "1m")]
    public async Task<ActionResult<IEnumerable<UserDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var page = await userService.GetAllAsync(paging, sorting, null);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserDO(e)));
    }

    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("/users/{id:guid}"), OutputCache(PolicyName = "1m")]
    public async Task<ActionResult<UserDO>> GetUser(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        return user is null ? NotFound() : Ok(new UserDO(user));
    }

    // [ProducesResponseType<UserDO>(StatusCodes.Status200OK)]
    [HttpPatch("/users/{id:guid}"), Authorize]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDO>> UpdateUser(Guid id, [FromBody] UserPatchRequestDTO data)
    {
        if (User.GetSID() != id && !User.IsAdmin())
            return Forbid();

        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound();

        user.DisplayName = data.DisplayName ?? user.DisplayName;
        user.AvatarUrl = data.AvatarUrl ?? user.AvatarUrl;
        await userService.UpdateAsync(user);
        return Ok(new UserDO(user));
    }

    [HttpPut("/users/{id:guid}/details"), Authorize]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDO>> UpdateUserDetails(Guid id, [FromBody] UserDetailsPutRequestDTO data)
    {
        var userID = User.GetSID();
        if (userID != id && !User.IsAdmin())
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

    [HttpGet("/users/{id:guid}/bio")]
    [EndpointSummary("Gets the user defined biography")]
    [EndpointDescription("A user can have a markdown biography to present themselves to others.")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, contentType: "text/plain")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDO>> GetUserBio(Guid id)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound();
        if (user.Details is null)
            return NoContent();
        return Ok(user.Details.Bio);
    }

    [HttpPut("/users/{id:guid}/bio")]
    [Consumes("text/plain")]
    [EndpointSummary("Sets the user defined biography")]
    [EndpointDescription("A user can have a markdown biography to present themselves to others.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetUserBio(Guid id, [FromBody] string markdown)
    {
        var user = await userService.UpsertDetails(id, new()
        {
            Bio = markdown
        });

        if (user is null)
            return NotFound();

        return Created();
    }

    [HttpGet("/users/{id:guid}/cursus")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserCursusDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserCursus(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        return Ok();
        // var cursi = await userService.GetUserCursi(user, pagination);
        // return Ok(cursi.Items.Select(c => new UserCursusDO(c)));
    }

    [HttpGet("/users/{id:guid}/goals")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserGoalDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserGoals(Guid id, [FromQuery] PaginationParams pagination)
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        return Ok();
        // var cursi = await userService.GetUserGoals(user, pagination);
        // return Ok(cursi.Items.Select(c => new UserGoalDO(c)));
    }


    [HttpGet("/users/{id:guid}/projects")]
    [EndpointSummary("Get the projects this user is subscribed to.")]
    [EndpointDescription("")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProjects(
        Guid id,
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[name]"), Description("Filter user projects based on the project name")] string? name,
        [FromQuery(Name = "filter[state]"), Description("Give projects that are in the following state")] TaskState? state,
        [FromQuery(Name = "filter[not[state]]"), Description("Give projects that are not in the following state")] TaskState? notState,
        [FromQuery(Name = "filter[slug]"), Description("Filter the user projects based on the project slug")] string? slug
    )
    {
        var user = await userService.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not found");

        var filters = new FilterDictionary()
            .AddFilter("name", name)
            .AddFilter("state", state)
            .AddFilter("user_id", id)
            .AddFilter("not_state", notState)
            .AddFilter("slug", slug);

        var page = await userProjectService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(e => new UserProjectDO(e)));
    }

    [HttpPost("/users/{id:guid}/projects/{projectId:guid}")]
    [EndpointSummary("Subscribe a user to a project")]
    [EndpointDescription("Subscribe a user to a project")]
    [ProducesResponseType<UserProjectDO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubscribeToProject(Guid id, Guid projectId)
    {
        var userID = User.GetSID();
        if (userID != id && !User.IsAdmin())
            return Forbid();

        var userProject = await userService.SubscribeToProject(userID, projectId);
        return Ok(new UserProjectDO(userProject));
    }

    [HttpDelete("/users/{id:guid}/projects/{projectId:guid}")]
    [EndpointSummary("Unsubscribe a user from a project")]
    [EndpointDescription("Unsubscribe a user from a project")]
    [ProducesResponseType<UserProjectDO>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UnsubscribeToProject(Guid id, Guid projectId)
    {
        var userID = User.GetSID();
        if (userID != id && !User.IsAdmin())
            return Forbid();

        var userProject = await userService.UnsubscribeFromProject(userID, projectId);
        return Ok(new UserProjectDO(userProject));
    }

    // TODO: /users/{id:guid}/cursus{cursusId:guid} -> GET, POST, DELETE,
    // TODO: /users/{id:guid}/goals{goalId:guid}
    // TODO: /users/{id:guid}/projects{projectId:guid}

    // ============================================================================
    // User Projects (a.k.a Project Instances)
    // ============================================================================

    [Tags(["Project Instances"])]
    [HttpGet("/users/projects/")]
    [EndpointSummary("Get all project instances that exist")]
    [EndpointDescription(@"When user's subscribe to a project they create their own unique instances.
    Instance can also be *shared* amongst users as can do projects together.")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProjects(
        [FromQuery] PaginationParams pagination,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[id]"), Description("Filter on user")] Guid? id,
        [FromQuery(Name = "filter[created_at]"), Description("Filter on project")] DateTimeOffset? createdAt,
        [FromQuery(Name = "filter[updated_at]"), Description("Filter on project")] DateTimeOffset? updatedAt,
        [FromQuery(Name = "filter[user_id]"), Description("Filter on user")] Guid? userId,
        [FromQuery(Name = "filter[project_id]"), Description("Filter on project")] Guid? projectId
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("user_id", id)
            .AddFilter("created_at", createdAt)
            .AddFilter("updated_at", updatedAt)
            .AddFilter("user_id", userId)
            .AddFilter("project_id", projectId);

        var page = await userProjectService.GetAllAsync(pagination, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(up => new UserProjectDO(up)));
    }

    [Tags(["Project Instances"])]
    [HttpGet("/users/projects/{id:guid}")]
    [EndpointSummary("")]
    [EndpointDescription("")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProject(Guid id, [FromQuery] PaginationParams pagination)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
    }

    [Tags(["Project Instances"])]
    [HttpPut("/users/projects/{id:guid}/git")]
    [EndpointSummary("Set the Git info")]
    [EndpointDescription("Set the git info")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetUserProjectGit(Guid id)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
    }

    [Tags(["Project Instances"])]
    [HttpPost("/users/projects/{id:guid}/invitation")]
    [EndpointSummary("Send a invitation to a user project")]
    [EndpointDescription("Send a invitation to a user project")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InviteUserToUserPorject(Guid id)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
    }

    [Tags(["Project Instances"])]
    [HttpDelete("/users/projects/{id:guid}/invitation")]
    [EndpointSummary("Decline a invitation to a user project")]
    [EndpointDescription("Decline a invitation to a user project")]
    [ProducesResponseType<UserProjectDO[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeclineInvitationToUserPorject(Guid id)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
    }
}
