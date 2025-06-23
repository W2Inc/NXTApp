// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Models.Requests.Review;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

[ApiController]
[Route("feedback"), Authorize]
public class FeedbackController(
    ILogger<ReviewController> logger,
    IFeedbackService service,
    ICommentService comments
) : Controller
{
    [Tags("Feedback")]
    [HttpPost("feedback")]
    [EndpointSummary("Add feedback to a review")]
    [EndpointDescription("Allows you to attach more feedback onto a exisiting review")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> Post([FromBody] FeedbackPostRequestDTO data)
    {
        logger.LogInformation("Received feedback request: {@FeedbackData}", data);
        if (data is FeedbackWithAnnotationDTO)
        {

        }

        return Ok();
        // var entity = await service.FindByIdAsync(id);
        // if (entity is null)
        //     return NotFound();
        // return Ok(new FeedbackDO(entity));
    }

    [HttpGet("/feedback/{id:guid}")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<ActionResult<FeedbackDO>> Get(Guid id)
    {
        var entity = await service.FindByIdAsync(id);
        if (entity is null)
            return NotFound();
        return Ok(new FeedbackDO(entity));
    }

    [HttpGet("/feedback/{id:guid}/comments")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<CommentDO>>> GetComments(
        Guid id,
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting
    )
    {
        var entity = await service.FindByIdAsync(id);
        if (entity is null)
            return NotFound();

        var filters = new FilterDictionary()
            .AddFilter("parent_id", entity.Id);

        var page = await comments.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new CommentDO(c)));
    }

    [HttpPost("/feedback/{id:guid}/comments")]
    [EndpointSummary("Update a goal")]
    [EndpointDescription("Updates a goal partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> AddComments(
        Guid id,
        [FromBody] CommentPutRequestDTO data,
        IUserService userService
    )
    {
        var entity = await service.FindByIdAsync(id);
        if (entity is null)
            return NotFound();

        var userId = data.UserId ?? User.GetSID();
        if (data.UserId.HasValue)
        {
            var user = await userService.FindByIdAsync(data.UserId.Value);
            if (user is null)
                return UnprocessableEntity("User does not exist");
            if (user.Id != User.GetSID() && User.IsAdmin())
                return Forbid();
        }

        await comments.CreateAsync(new()
        {
            UserId = userId,
            Markdown = data.Markdown,
            ParentId = entity.Id,
            ParentType = nameof(Feedback)
        });

        return Created();
    }
}
