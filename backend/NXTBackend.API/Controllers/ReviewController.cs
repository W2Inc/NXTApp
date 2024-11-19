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
using NXTBackend.API.Models.Requests.Review;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

// public class ReviewQueryParams
// {
//     [Description("URL slug to filter on")]
//     [FromQuery(Name = "filter[slug]")]
//     public string? Slug { get; set; }

//     [Description("The entity id to filter on")]
//     [FromQuery(Name = "filter[id]")]
//     public Guid? Id { get; set; }
// }

// ============================================================================

[ApiController]
[Route("reviews"), Authorize]
public class ReviewController(
    ILogger<ReviewController> logger,
    IReviewService reviewService
    // IUserProjectService userProjectService
) : Controller
{
    [HttpGet("/reviews"), AllowAnonymous]
    [EndpointSummary("Get all exisiting goals")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var page = await reviewService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new ReviewDO(c)));
    }

    [HttpPost("/reviews")]
    [EndpointSummary("Create a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK),]
    public async Task<ActionResult<LearningGoalDO>> Create([FromBody] ReviewPostRequestDto data)
    {
        // If neither exists, then user who is making the request is asking to *be* reviewed.
        // In return the review we create will have a pending state
        if (data.Kind is null && data.ReviewerId is null)
        {
            // First check that the user requesting this review
            // is actually a member or an admin

        }
            // return UnprocessableEntity(new ProblemDetails() { Title = "" });

        var review = await reviewService.CreateAsync(new()
        {

        });

        return Ok(new ReviewDO(review));
    }

    [HttpGet("/goals/{id:guid}"), AllowAnonymous]
    [EndpointSummary("Get a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    public async Task<ActionResult<LearningGoalDO>> Deprecate(Guid id)
    {
        var goal = await goalService.FindByIdAsync(id);
        if (goal is null)
            return NotFound("Cursus not found");

        await goalService.DeleteAsync(goal);
        return Ok(new LearningGoalDO(goal));
    }

    [HttpGet("/goals/{id:guid}/projects"), AllowAnonymous]
    [EndpointSummary("Get the projects that are part of this goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LearningGoalDO>>> GetProjects(
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
        return Ok(page.Items.Select(p => new SimpleUserDO(p)));
    }
}
