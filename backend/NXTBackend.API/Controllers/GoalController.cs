// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.Query;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using NXTBackend.API.Models.Requests.LearningGoal;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

public class GoalQueryParams
{
    [Description("Filter on names")]
    [FromQuery(Name = "filter[name]")]
    public string? Name { get; set; }

    [Description("URL slug to filter on")]
    [FromQuery(Name = "filter[slug]")]
    public string? Slug { get; set; }

    [Description("The entity id to filter on")]
    [FromQuery(Name = "filter[id]")]
    public Guid? Id { get; set; }
}

// ============================================================================

[ApiController]
[Route("goals"), Authorize]
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
        var page = await goalService.GetAllAsync(paging, sorting);

        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new LearningGoalDO(c)));
    }

    [HttpPost("/goals")]
    [EndpointSummary("Create a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Create([FromBody] GoalPostRequestDto data)
    {

        var goal = await goalService.CreateAsync(new()
        {
            CreatorId = User.GetSID(),
            Markdown = data.Markdown,
            Name = data.Name,
            Slug = data.Name.ToUrlSlug(),
            Description = data.Description,
        });

        return Ok(new LearningGoalDO(goal));
    }

    [HttpGet("/goals/{id:guid}"), AllowAnonymous]
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

    [HttpPatch("/goals/{id:guid}")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Update(Guid id, [FromBody] GoalPatchRequestDto data)
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound();
        if (User.GetSID() != goal.CreatorId && !User.IsAdmin())
            return Forbid();

        if (data.Markdown is not null)
            goal.Markdown = data.Markdown;
        if (data.Description is not null)
            goal.Description = data.Description;
        if (data.Name is not null)
        {
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
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound("Cursus not found");

        await goalService.DeleteAsync(goal);
        return Ok(new LearningGoalDO(goal));
    }

    [HttpGet("/goals/{id:guid}/projects")]
    [EndpointSummary("Get the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProjectDO>>> GetProjects(
        Guid id,
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound("Cursus not found");

        var page = await goalService.GetProjects(goal, paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(p => new ProjectDO(p)));
    }

    [HttpPut("/goals/{id:guid}/projects")]
    [EndpointSummary("Set the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProjectDO>>> SetProjects(
        Guid id,
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound("Cursus not found");

        var page = await goalService.GetProjects(goal, paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(p => new ProjectDO(p)));
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
