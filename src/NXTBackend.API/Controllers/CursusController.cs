using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Common;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Requests.Auth;

namespace NXTBackend.API.Controllers;

[Route("cursus")]
[ApiController]
public class CursusController(IUserService userService) : ControllerBase
{
    private readonly IUserService _searchService = userService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus")]
    public async Task<IActionResult> GetCursi([FromQuery] PaginationParams pagination)
    {

        return BadRequest("WUAH!");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpPost("/cursus"), Authorize]
    public async Task<IActionResult> AddCursi([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus/{id}")]
    public async Task<IEnumerable<User>> GetCursus([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpPatch("/cursus/{id}"), Authorize]
    public async Task<IEnumerable<User>> SetCursus([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus/{id}/users")]
    public async Task<IEnumerable<User>> GetUsers([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus/{id}/goals")]
    public async Task<IEnumerable<LearningGoal>> GetGoals([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }
}
