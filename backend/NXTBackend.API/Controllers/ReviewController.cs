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
using NXTBackend.API.Core.Utils.Query;
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
[Route("reviews")]
public class ReviewController(
    ILogger<ReviewController> logger,
    IReviewService reviewService,
    IRubricService rubricService,
    IUserProjectService userProjectService,
    IUserService userService
) : Controller
{
    [HttpGet("/reviews"), AllowAnonymous]
    [EndpointSummary("Get all exisiting goals")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ReviewDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery(Name = "filter[user_project_id]")] Guid userProjectId,
        [FromQuery] SortingParams sorting
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("user_project_id", userProjectId);

        var page = await reviewService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new ReviewDO(c)));
    }

    [HttpPost("/reviews"), Consumes("application/json")]
    [EndpointSummary("Create a review")]
    [EndpointDescription(@"
If the kind of review and reviewerId are null. It will signal that the project instance is *requesting* a review"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Create(ReviewPostRequestDTO requestedReview)
    {
        // 1. Check user project
        logger.LogCritical("{@shit}", requestedReview);
        if (requestedReview.RubricId == Guid.Empty)
            return UnprocessableEntity(new ProblemDetails() { Title = "Rubric does not exist" });

        // TODO: Check for exisiting reviews

        var userProject = await userProjectService.FindByIdAsync(requestedReview.UserProjectId);
        if (userProject is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Project instance does not exist" });
        if (userProject.GitInfoId is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Project has no git repository specified" });

        var rubric = await rubricService.FindByIdAsync(requestedReview.RubricId);
        if (rubric is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Rubric does not exist" });
        if (userProject.ProjectId != rubric.ProjectId)
            return UnprocessableEntity(new ProblemDetails() { Title = "Rubric can't be used for project instance" });

        bool userIsMember = userProject.Members.Any(m => m.UserId == requestedReview.ReviewerId);
        switch (requestedReview.Kind)
        {
            case ReviewKind.Self:
                if (!userIsMember)
                    return UnprocessableEntity(new ProblemDetails() { Title = "Reviewer must be a member of project instance" });
                break;
            case ReviewKind.Peer:
            case ReviewKind.Async:
                // TODO: Check if request has the privilege to assign user to a review
                if (requestedReview.ReviewerId is not null)
                {
                    if (userIsMember)
                        return UnprocessableEntity(new ProblemDetails() { Title = "Reviewer must not be a member of project instance" });
                    // var user = userService.FindByIdAsync(requestedReview.ReviewerId.Value);
                    // if (user is null)
                    //     return UnprocessableEntity(new ProblemDetails() { Title = "User does not exist" });
                }
                break;
            case ReviewKind.Auto:
                throw new ServiceException(StatusCodes.Status501NotImplemented, "Auto reviews are not yet implemented");
            default:
                throw new NotImplementedException($"Review kind: {requestedReview.Kind} is not supported.");
        }

        return Ok(new ReviewDO(await reviewService.CreateAsync(new()
        {
            Kind = requestedReview.Kind,
            State = ReviewState.Pending,
            ReviewerId = requestedReview.Kind is ReviewKind.Self ? requestedReview.ReviewerId : null,
            UserProjectId = requestedReview.UserProjectId,
            RubricId = requestedReview.RubricId
        })));
    }

    [HttpGet("/reviews/{id:guid}"), AllowAnonymous]
    [EndpointSummary("Get a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Get(Guid id)
    {
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound();
        return Ok(new ReviewDO(review));
    }

    [HttpPatch("/reviews/{id:guid}")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Update(Guid id, [FromBody] ReviewPatchRequestDto data)
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
        // return Ok(new ReviewDO(updatedGoal));
    }


    [HttpDelete("/reviews/{id:guid}")]
    [EndpointSummary("Delete a goal")]
    [EndpointDescription("Goal deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Deprecate(Guid id)
    {
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound("Cursus not found");

        await reviewService.DeleteAsync(review);
        return Ok(new ReviewDO(review));
    }
}
