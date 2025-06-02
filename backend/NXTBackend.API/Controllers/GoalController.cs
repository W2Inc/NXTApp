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
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using NXTBackend.API.Models.Requests.LearningGoal;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

// ============================================================================

[ApiController]
[Route("goals"), Authorize]
[ProducesErrorResponseType(typeof(ProblemDetails))]
public class GoalController(
    ILogger<GoalController> logger,
    IGoalService goalService,
    IProjectService projectService
) : Controller
{
    [HttpGet("/goals")]
    [EndpointSummary("Get all exisiting goals")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<LearningGoalDO>>> GetAll(
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

        var page = await goalService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new LearningGoalDO(c)));
    }

    [HttpPost("/goals"), Authorize(Policy = "CanCreate")]
    [EndpointSummary("Create a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LearningGoalDO>> Create([FromBody] GoalPostRequestDto data)
    {
        var existingGoal = await goalService.Query()
            .Where(g => g.Name.ToLower() == data.Name.ToLower())
            .FirstOrDefaultAsync();

        if (existingGoal is not null)
            return Conflict();

        var goal = await goalService.CreateAsync(new LearningGoal
        {
            CreatorId = User.GetSID(),
            Markdown = data.Markdown,
            Name = data.Name,
            Slug = data.Name.ToUrlSlug(),
            Description = data.Description,
        });

        return Ok(new LearningGoalDO(goal));
    }

    [HttpGet("/goals/{id:guid}")]
    [EndpointSummary("Get a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Get(Guid id)
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound();
        return Ok(new LearningGoalDO(goal));
    }

    [HttpPatch("/goals/{id:guid}"), Authorize(Policy = "CanCreate")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LearningGoalDO>> Update(Guid id, [FromBody] GoalPatchRequestDto data)
    {
        var (goal, user) = await goalService.IsCollaborator(id, User.GetSID());
        if (goal is null)
            return NotFound();
        if (user is null)
            return Forbid();

        if (data.Markdown is not null)
            goal.Markdown = data.Markdown;
        if (data.Description is not null)
            goal.Description = data.Description;
        if (data.Name is not null)
        {
            var existingGoal = await goalService.Query()
                .Where(g => g.Name.ToLowerInvariant() == data.Name.ToLowerInvariant())
                .FirstOrDefaultAsync();

            if (existingGoal is not null)
                return Conflict();
            goal.Name = data.Name;
            goal.Slug = goal.Name.ToUrlSlug();
        }

        var updatedGoal = await goalService.UpdateAsync(goal);
        return Ok(new LearningGoalDO(updatedGoal));
    }


    [HttpDelete("/goals/{id:guid}")]
    [EndpointSummary("Delete a goal")]
    [EndpointDescription("Goal deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Deprecate(Guid id)
    {
        var (goal, user) = await goalService.IsCollaborator(id, User.GetSID());
        if (goal is null)
            return NotFound();
        if (user is null)
            return Forbid();

        await goalService.DeleteAsync(goal);
        return Ok(new LearningGoalDO(goal));
    }

    [HttpGet("/goals/{id:guid}/projects")]
    [EndpointSummary("Get the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProjectDO>>> GetProjects(Guid id, [FromQuery] SortingParams sorting)
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound();

        var projects = await goalService.GetProjects(goal, sorting);
        return Ok(projects.Select(p => new ProjectDO(p)));
    }

    [HttpPut("/goals/{id:guid}/projects")]
    [EndpointSummary("Set the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProjectDO>>> SetProjects(Guid id, [FromBody] IEnumerable<Guid> projects)
    {
        var (goal, user) = await goalService.IsCollaborator(id, User.GetSID());
        if (goal is null)
            return NotFound();
        if (user is null)
            return Forbid();
        
        var entities = await goalService.SetProjects(goal, projects);
        return Ok(entities.Select(p => new ProjectDO(p)));
    }

    [HttpGet("/goals/{id:guid}/users"), AllowAnonymous]
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
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound("Cursus not found");

        var page = await goalService.GetUsers(goal, paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(p => new MinimalUserDTO(p)));
    }
}