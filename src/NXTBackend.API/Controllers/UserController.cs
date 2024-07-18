using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.User;

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController]
public class UserController(ILogger<UserController> logger, IUserService userService) : ControllerBase
{
    private readonly IUserService _searchService = userService;

    /// <summary>
    /// Get the currently authenticated user
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<User>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/me")]
    public async Task<IActionResult> GetMe()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the currently authenticated user's events
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<Event>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/me/events")]
    public async Task<IActionResult> GetMeEvents([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the currently authenticated user
    /// </summary>
    /// <response code="204">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType(204)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpDelete("/me/events/{id}")]
    public async Task<IActionResult> DeleteMeEvent(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all the users
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<User>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/user")]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all the users
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<User>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/user/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a specific user by id
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<User>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPatch("/user/{id}")]
    public async Task<IActionResult> SetUser(Guid id, [FromBody] UserPatchRequestDto body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update the user details of a user
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Details>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPatch("/user/{id}/details")]
    public async Task<IActionResult> SetUserDetails(Guid id, [FromBody] BaseRequestDto body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all the user cursi of a user
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<UserCursus>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/user/{id}/user_cursus")]
    public async Task<IActionResult> GetUserCursus(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all the user goals of a user
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<UserGoal>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/user/{id}/user_goals/")]
    public async Task<IActionResult> GetUserGoals(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all the user projects of a user
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<IEnumerable<UserProject>>(200)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpGet("/user/{id}/user_projects")]
    public async Task<IActionResult> GetUserProjects(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Invite the given user to a project
    /// </summary>
    /// <response code="204">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType(204)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPost("/user/{id}/user_projects/{user_project_id}/invite")] // Send
    public async Task<IActionResult> InviteUserToProject(Guid id, Guid user_project_id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Accept the invite to a project
    /// </summary>
    /// <response code="204">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType(204)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpPost("/user/{id}/user_projects/{user_project_id}/invite/accept")] // Accept
    public async Task<IActionResult> AcceptUserToProject(Guid id, Guid user_project_id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Decline the invite to a project
    /// </summary>
    /// <response code="204">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType(204)]
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [HttpDelete("/user/{id}/user_projects/{user_project_id}/invite")] // Decline
    public async Task<IActionResult> DeclineUserToProject(Guid id, Guid user_project_id)
    {
        throw new NotImplementedException();
    }
}
