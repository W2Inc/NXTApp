// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Requests.Review;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

[ApiController]
[Route("reviews"), Authorize]
public class ReviewController(
    ILogger<ReviewController> logger,
    IReviewService reviewService,
    IRubricService rubricService,
    IUserProjectService userProjectService,
    IUserService userService
) : Controller
{
    [HttpGet("/reviews")]
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
Establishes a review on the given user project, but does not start it immedieatly.
    ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Request([FromBody] ReviewRequestPostRequestDTO data, IGitService gitService)
    {
        var up = await userProjectService.FindByIdAsync(data.UserProjectId);
        if (up is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Project instance does not exist" });
        if (up.GitInfoId is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Project has no git repository specified" });

        var rubric = await rubricService.FindByIdAsync(data.RubricId);
        if (rubric is null)
            return UnprocessableEntity(new ProblemDetails() { Title = "Rubric does not exist" });
        if (up.ProjectId != rubric.ProjectId)
            return UnprocessableEntity(new ProblemDetails() { Title = "Rubric can't be used on this project" });

        bool userIsMember = up.Members.Any(m => m.UserId == data.ReviewerId);
        switch (data.Kind)
        {
            case ReviewKind.Self:
                if (!userIsMember)
                    return UnprocessableEntity(new ProblemDetails() { Title = "Reviewer must be a member of project instance" });
                break;
            case ReviewKind.Peer:
            case ReviewKind.Async:
                // TODO: Check if request has the privilege to assign user to a review
                if (data.ReviewerId is not null)
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
                throw new NotImplementedException($"Review kind: {data.Kind} is not supported.");
        }

        return Ok(new ReviewDO(await reviewService.CreateAsync(new()
        {
            Kind = data.Kind,
            State = ReviewState.Pending,
            Hash = await gitService.GetLatestHash(up.GitInfoId.Value),
            ReviewerId = data.Kind is ReviewKind.Self ? data.ReviewerId : null,
            UserProjectId = data.UserProjectId,
        })));
    }


    [HttpPost("/reviews/{id:guid}/start"), Consumes("application/json")]
    [EndpointSummary("Start a review")]
    [EndpointDescription(@"
Basically, once evaluated, this endpoint can be used to finalize the review / marking it completed
Allows directly uploading the annotations / corrections of files.

Calling this endpoint on a review marks it as completed.
    ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Start(Guid id)
    {
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound();

        // For Auto reviews, there is no reviewer, in other cases check if it set yet.
        // Obviously we can't review the user project if no one is there to review it...
        if (review.Kind is not ReviewKind.Auto && review.ReviewerId is null)
            return UnprocessableEntity("Can't start review without a reviewer");
        // Caller must be user and if not must be staff
        if (review.ReviewerId != User.GetSID() && !User.IsAdmin())
            return Forbid();
        if (review.Kind is ReviewKind.Peer)
        {
            // TODO: Verify the IP Address to ensure it comes from within the Network only
            return Problem(title: "TODO: IPAddress check", statusCode: StatusCodes.Status501NotImplemented);
        }
        if (review.Kind is ReviewKind.Auto)
        {
            // TODO: Dispatch a Job, and setup webhook routes to inform us on when the tests are done running
            // NXTPeer is "done" but we have no webhooks yet
            return Problem(title: "TODO: NXTPeer service", statusCode: StatusCodes.Status501NotImplemented);
        }

        // Async and Self can just go ahead
        review.State = ReviewState.InProgress;
        await reviewService.UpdateAsync(review);
        return Ok();
    }

    [HttpPost("/reviews/{id:guid}/finish"), Consumes("application/json")]
    [EndpointSummary("Submit a finalization of the review")]
    [EndpointDescription(@"
Basically, once evaluated, this endpoint can be used to finalize the review / marking it completed
Allows directly uploading the annotations / corrections of files.

Calling this endpoint on a review marks it as completed.
    ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDO>> Finish(Guid id, [FromBody] ReviewFinalizePostRequestDTO data)
    {
        return Ok();
    }

    [HttpGet("/reviews/{id:guid}")]
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

    [HttpDelete("/reviews/{id:guid}/cancel")]
    [EndpointSummary("Cancels a review.")]
    [EndpointDescription("Deletes a review / cancels it")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Cancel(Guid id)
    {
        // TODO: Notify the members that a review on their project has been cancelled
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound("Cursus not found");

        await reviewService.DeleteAsync(review);
        return Ok(new ReviewDO(review));
    }

    [HttpDelete("/reviews/{id:guid}")]
    [EndpointSummary("Deletes a review.")]
    [EndpointDescription("Deletes a review / cancels it")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LearningGoalDO>> Delete(Guid id)
    {
        // TODO: Notify the members that a review on their project has been cancelled
        var review = await reviewService.FindByIdAsync(id);
        if (review is null)
            return NotFound("Cursus not found");

        await reviewService.DeleteAsync(review);
        return Ok(new ReviewDO(review));
    }
}
