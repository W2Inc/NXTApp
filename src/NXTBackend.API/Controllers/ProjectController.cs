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

[Route("project")]
[ApiController]
public class ProjectController(IUserService userService) : ControllerBase
{
    private readonly IUserService _searchService = userService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project")]
    public async Task<IEnumerable<Project>> GetProjects([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("/project"), Authorize]
    public async Task<IActionResult> AddProject()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project/{id}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project/{id}/users")]
    public async Task<IActionResult> GetUsersForProject(
        [FromQuery] PaginationParams pagination,
        Guid id
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project/{id}/rubrics")]
    public async Task<IActionResult> GetProjectRubrics(
        [FromQuery] PaginationParams pagination,
        Guid id
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project/{id}/git")]
    public async Task<IActionResult> GetProjectGitInfo(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("/project/{id}/git"), Authorize]
    public async Task<IActionResult> SetProjectGitInfo(Guid id)
    {
        throw new NotImplementedException();
    }
}
