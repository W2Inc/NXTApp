// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
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
    IResourceOwnerService ownerService,
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

    [HttpPost("/projects"), Authorize(Policy = "CanCreate")]
    [EndpointSummary("Create a project")]
    [EndpointDescription("Creates a new project, also creates a remote repository for hosting the project.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ProjectDO>> Create([FromBody] ProjectPostRequestDTO data, IDistributedCache cache)
    {
        var owner = await ownerService.FindByIdAsync(data.OwnerId);
        if (owner is null)
            return UnprocessableEntity("Non-existing user");

        var git = await gitService.CreateRepository(new()
        {
            Name = data.Name,
            Description = data.Description,
        }, owner.Type);

        await gitService.SetFile(
            git.Id,
            "readme.md",
            data.Markdown,
            "Initial Commit"
        );

        var project = await projectService.CreateProjectWithGit(new()
        {
            CreatorId = User.GetSID(),
            Name = data.Name,
            Slug = data.Name.ToUrlSlug(),
            Description = data.Description,
            MaxMembers = data.MaxMembers,
            ThumbnailUrl = data.ThumbnailUrl,
            Tags = data.Tags
        }, git);

        await cache.SetStringAsync($"M_{project.Id}", data.Markdown);
        return Ok(new ProjectDO(project));
    }

    [HttpGet("/projects/{id:guid}")]
    [EndpointSummary("Get a project")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Get(Guid id)
    {
        // var project = await projectService.FindByIdAsync(id);


        var (project, user) = await projectService.IsCollaborator(id, User.GetSID());
        logger.LogInformation("==> Is User {UserId} a collaborator on project {ProjectId}: {isCollaborator}",
            User.GetSID(),
            id,
            user is not null
            );

        if (project is null)
            return NotFound();
        return Ok(new ProjectDO(project));
    }

    [HttpGet("/projects/{id:guid}/markdown/{file:regex(^(README|SUBJECT)\\.(md|MD|Md|mD)$)}")]
    [EndpointSummary("Get a markdown of the project")]
    [EndpointDescription(@"
Retrieves the current markdown file, may either be the README or the SUBJECT.
Subject is the actual subject sheet while the readme is simply an explanation of the project itself.
    ")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, contentType: "text/markdown")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [OutputCache(Duration = 300)] // Cache for 5 minutes
    public async Task<ActionResult<string>> GetMarkdown(
        Guid id,
        string file,
        [FromQuery(Name = "filter[branch]")] string branch = "main"
    )
    {
        // NOTE(W2): Only support UTF-8 Strings for now.
        var content = await projectService.GetFileFromProject(id, file, branch);
        if (content is null)
            return NotFound();
        return Ok(content);
    }

    [HttpPatch("/projects/{id:guid}"), Authorize(Policy = "CanCreate")]
    [EndpointSummary("Update a project")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectDO>> Update(Guid id, [FromBody] ProjectPatchRequestDto data)
    {
        var (project, user) = await projectService.IsCollaborator(id, User.GetSID());
        if (project is null)
            return NotFound();
        if (user is null)
            return Forbid();

        // if (data.Markdown is not null)
            //     project.Markdown = data.Markdown;
            if (data.Description is not null)
                project.Description = data.Description;
        if (data.Name is not null)
        {
            project.Name = data.Name;
            project.Slug = project.Name.ToUrlSlug();
        }

        if (data.Markdown is not null)
        {
            await gitService.SetFile(project.GitInfoId, "README.md", data.Markdown, "Update Readme");
        }

        var updatedProject = await projectService.UpdateAsync(project);
        // await gitService.UpsertFile(updatedProject.Creator.Login, project.Name, "README.md", data.Markdown, "Update README.md");
        return Ok(new ProjectDO(updatedProject));
    }


    [HttpDelete("/projects/{id:guid}"), Authorize(Policy = "CanCreate")]
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
