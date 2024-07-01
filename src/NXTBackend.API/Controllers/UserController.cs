using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Common;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Requests.Auth;
using Serilog;

namespace NXTBackend.API.Controllers;

[Route("user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _searchService = userService;
    private readonly Serilog.ILogger _log = Log.Logger.ForContext<UserController>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    /// <response code="200">User created</response>
    /// <response code="500">An Internal server error has occurred</response>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/user")]
    public async Task<IEnumerable<User>> GetUsers([FromQuery] PaginationParams pagination)
    {
        var users = await _searchService.GetAllAsync(pagination);
        return users.Items;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/user/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _searchService.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/user/{id}/events")]
    public async Task<IActionResult> GetEventsOfUser(Guid id)
    {
        return StatusCode(501);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/user/{id}/details")]
    public async Task<IActionResult> GetUserDetails(Guid id)
    {
        return StatusCode(501);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("/user/{id}/details"), Authorize]
    public async Task<IActionResult> SetUserDetails(Guid id, [FromBody]UserPatchRequestDto patch)
    {
        var user = await _searchService.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        user.AvatarUrl = patch.AvatarUrl;
        user.DisplayName = patch.DisplayName;
        user = await _searchService.UpdateAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("/user/{id}/events/{eventId}"), Authorize]
    public async Task<IActionResult> DeleteEventForUser(Guid id, Guid eventId)
    {
        var user = await _searchService.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        return StatusCode(501);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await _searchService.FindByLoginAsync(User.Identity.Name);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
}
