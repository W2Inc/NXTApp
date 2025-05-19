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
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Enums;
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
[Route("rubrics")]
public class RubricsController(
    ILogger<ReviewController> logger,
    IRubricService rubricService
) : Controller
{
    [HttpGet("/rubrics")]
    [EndpointSummary("Get all exisiting rubrics")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RubricDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[name]")] string? name,
        [FromQuery(Name = "filter[project_id]")] string? projectId,
        [FromQuery(Name = "filter[project_slug]")] string? projectSlug
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("name", name)
            .AddFilter("project_id", projectId)
            .AddFilter("project_slug", projectSlug);

        var page = await rubricService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new RubricDO(c)));
    }

    [HttpPost("/rubrics"), Consumes("application/json")]
    [EndpointSummary("Create a rubric")]
    [EndpointDescription("Creates a new rubric")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RubricDO>> Create(ReviewPostRequestDTO requestedReview)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
    }

    [HttpGet("/rubrics/{id:guid}")]
    [EndpointSummary("Get a specific rubric")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RubricDO>> Get(Guid id)
    {
        var review = await rubricService.FindByIdAsync(id);
        if (review is null)
            return NotFound();
        return Ok(new RubricDO(review));
    }

    [HttpPatch("/rubrics/{id:guid}")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RubricDO>> Update(Guid id, [FromBody] ReviewPatchRequestDto data)
    {
        throw new ServiceException(StatusCodes.Status501NotImplemented, "TODO");
        // var goal = await reviewService.FindByIdAsync(id);
        // if (goal is null)
        //     return NotFound();
        // if (User.GetSID() != goal.CreatorId && !User.IsAdmin())
        //     return Forbid();

        // if (data.Markdown is not null)
        //     goal.Markdown = data.Markdown;
        // if (data.Description is not null)
        //     goal.Description = data.Description;
        // if (data.Name is not null)
        // {
        //     goal.Name = data.Name;
        //     goal.Slug = goal.Name.ToUrlSlug();
        // }

        // var updatedGoal = await goalService.UpdateAsync(goal);
        // return Ok(new RubricDO(updatedGoal));
    }


    [HttpDelete("/rubrics/{id:guid}")]
    [EndpointSummary("Delete a goal")]
    [EndpointDescription("Goal deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RubricDO>> Deprecate(Guid id)
    {
        var rubric = await rubricService.FindByIdAsync(id);
        if (rubric is null)
            return NotFound("Cursus not found");
        return Ok(new RubricDO(await rubricService.DeleteAsync(rubric)));
    }
}
