// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using NXTBackend.API.Models.Requests.LearningGoal;
using NXTBackend.API.Models.Requests.Project;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

// ============================================================================

[ApiController, Authorize]
[Route("projects")]
public class ProjectController(
    ILogger<ProjectController> logger,
    IProjectService projectService,
    IGitService gitService
) : Controller
{
    [HttpGet("/projects")]
    [EndpointSummary("Get all exisiting projects")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProjectDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[id]")] Guid? id,
        [FromQuery(Name = "filter[slug]")] string? slug,
        [FromQuery(Name = "filter[name]")] string? name
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("id", id)
            .AddFilter("slug", slug)
            .AddFilter("name", name);

        var page = await projectService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new ProjectDO(c)));
    }

    [HttpPost("/projects"), Authorize(Roles = "creator")]
    [EndpointSummary("Create a project")]
    [EndpointDescription("Creates a new project, also creates a remote repository for hosting the project.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Create([FromBody] ProjectPostRequestDto data)
    {
        // 1. Create Git repository
        var git = await gitService.CreateRemoteRepository(data.Name, data.Description);
        var project = await projectService.CreateProjectWithGit(new()
        {
            CreatorId = User.GetSID(),
            Markdown = data.Markdown,
            Name = data.Name,
            Slug = data.Name.ToUrlSlug(),
            Description = data.Description,
            MaxMembers = data.MaxMembers,
            ThumbnailUrl = data.ThumbnailUrl,
            Tags = data.Tags
        }, git);

        return Ok(new ProjectDO(project));
    }

    [HttpGet("/projects/{id:guid}")]
    [EndpointSummary("Get a project")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Get(Guid id)
    {
        var project = await projectService.FindByIdAsync(id);
        if (project is null)
            return NotFound();
        return Ok(new ProjectDO(project));
    }

    [HttpPatch("/projects/{id:guid}"), Authorize(Roles = "creator")]
    [EndpointSummary("Update a project")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Update(Guid id, [FromBody] ProjectPatchRequestDto data)
    {
        var project = await projectService.FindByIdAsync(id);
        if (project is null)
            return NotFound();
        if (User.GetSID() != project.CreatorId)
            return Forbid();

        if (data.Markdown is not null)
            project.Markdown = data.Markdown;
        if (data.Description is not null)
            project.Description = data.Description;
        if (data.Name is not null)
        {
            project.Name = data.Name;
            project.Slug = project.Name.ToUrlSlug();
        }

        var updatedProject = await projectService.UpdateAsync(project);
        await gitService.UpsertFile(updatedProject.Creator.Login, project.Name, "README.md", data.Markdown, "Update README.md");
        return Ok(new ProjectDO(updatedProject));
    }


    [HttpDelete("/projects/{id:guid}"), Authorize(Roles = "creator")]
    [EndpointSummary("Delete a project")]
    [EndpointDescription("Goal deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Deprecate(Guid id)
    {
        var project = await projectService.FindByIdAsync(id);
        if (project is null)
            return NotFound("Project not found");

        await projectService.DeleteAsync(project);
        return Ok(new ProjectDO(project));
    }

    // [HttpGet("/projects/{id:guid}/goals"), AllowAnonymous]
    // [EndpointSummary("Get the projects that are part of this goal")]
    // [EndpointDescription("")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<IEnumerable<LearningGoalDO>>> GetProjects(
    //     Guid id,
    //     [FromQuery] PaginationParams paging,
    //     [FromQuery] SortingParams sorting
    // )
    // {
    //     var project = await projectService.FindByIdAsync(id);
    //     if (project is null)
    //         return NotFound("Cursus not found");

    //     var page = await projectService.Get(project, paging, sorting);
    //     page.AppendHeaders(Response.Headers);
    //     return Ok(page.Items.Select(p => new ProjectDO(p)));
    // }

    [HttpGet("/projects/{id:guid}/users")]
    [EndpointSummary("Get the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<LearningGoalDO>>> GetUsers(
        Guid id,
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var project = await projectService.FindByIdAsync(id);
        if (project is null)
            return NotFound("Project not found");

        var page = await projectService.GetUsers(project, paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(p => new MinimalUserDTO(p)));
    }
}
