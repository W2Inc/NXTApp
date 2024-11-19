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
[Route("reviews")]
public class ReviewController(
    ILogger<ReviewController> logger,
    IReviewService reviewService,
    IUserProjectService userProjectService,
    IUserService userService
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

    [HttpPost("/reviews"), Consumes("application/json")]
    [EndpointSummary("Create a review")]
    [EndpointDescription(@"
If the kind of review and reviewerId are null.It will signal that the project instance is *requesting* a review"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ReviewDO>> Create(ReviewPostRequestDTO data)
    {
        logger.LogCritical("{@shit}", data);
        // 1. If self review, the reviewerId in data must not be null and the kind be self
        // 2. If peer / async review
        //      - If we are requesting a review, requester must be a member and reviewerId is null
        //      - If we are trying to review an instance that is maybe requesting a review then make sure we are not a member
        // 3. If Auto then reviewerId will be that of the BO

        if (data.Kind is ReviewKind.Self && (data.ReviewerId is null || User.GetSID() != data.ReviewerId))
            throw new ServiceException("Reviewer ID is required and must be the authenticated user");

        var userProject = await userProjectService.FindByIdAsync(data.UserProjectId);
        if (userProject is null)
            throw new ServiceException("Project instance does not exist");

        bool userIsMember = userProject.Members.Any(m => m.UserId == User.GetSID());
        if (data.ReviewerId is null)
        {
            if (!userIsMember)
                throw new ServiceException(StatusCodes.Status403Forbidden, "You need to be a member of this project instance");

            var review = await reviewService.CreateAsync(new()
            {
                Kind = data.Kind,
                State = ReviewState.Pending,
                UserProjectId = data.UserProjectId,
            });

            return Ok(new ReviewDO(review));
        }

        if (userProject.Members.Any(m => m.UserId == data.ReviewerId))
            throw new ServiceException("Reviewer must not be a member of the project");
        if (await userService.FindByIdAsync(data.ReviewerId.Value) is not null)
            throw new ServiceException("Reviewer does not exist");

        return Ok(new ReviewDO(await reviewService.CreateAsync(new()
        {
            Kind = data.Kind,
            State = ReviewState.InProgress,
            ReviewerId = data.ReviewerId,
            UserProjectId = data.UserProjectId,
        })));
    }

    [HttpGet("/goals/{id:guid}"), AllowAnonymous]
    [EndpointSummary("Get a goal")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ReviewDO>> Get(Guid id)
    {
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound();
        return Ok(new ReviewDO(review));
    }

    [HttpPatch("/goals/{id:guid}")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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


    [HttpDelete("/goals/{id:guid}")]
    [EndpointSummary("Delete a goal")]
    [EndpointDescription("Goal deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LearningGoalDO>> Deprecate(Guid id)
    {
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound("Cursus not found");

        await reviewService.DeleteAsync(review);
        return Ok(new ReviewDO(review));
    }
}
