using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.User;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Project;

namespace NXTBackend.API.Controllers;

[Route("project")]
[ApiController]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;

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
    [HttpPost("/project")]
    [ProducesResponseType<Project>(200)]
    [ProducesResponseType<BaseResponseDto>(401)]
    [ProducesResponseType<BaseResponseDto>(403)]
    [ProducesResponseType<BaseResponseDto>(500)]
    public async Task<IActionResult> AddProject(ProjectPostRequestDto request)
    {
        var project = await _projectService.CreateAsync(new Project
        {

        });

        return Ok(project);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/project/{id}")]
    [ProducesResponseType<Project>(200)]
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
    public async Task<IEnumerable<User>> GetUsersForProject(
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
    public async Task<IEnumerable<Rubric>> GetProjectRubrics(
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
    [ProducesResponseType<Git>(200)]
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
    [ProducesResponseType<Git>(200)]
    public async Task<IActionResult> SetProjectGitInfo(Guid id)
    {
        throw new NotImplementedException();
    }
}
