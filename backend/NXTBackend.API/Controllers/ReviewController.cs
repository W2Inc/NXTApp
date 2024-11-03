using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Review;

namespace NXTBackend.API.Controllers;

[Route("reviews")]
[ApiController]
public class ReviewController(
    IReviewService reviewService,
    IFeedbackService feedbackService,
    ICommentService commentService,
    IProjectService projectService
) : ControllerBase
{

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/reviews")]
    public async Task<IEnumerable<Review>> GetReviews(
        [FromQuery] PaginationParams pagination,
        [FromQuery] SortingParams sorting
    )
    {
        return (await reviewService.GetAllAsync(pagination, sorting)).Items;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("/reviews")]
    public async Task<IActionResult> AddReview(
        [FromQuery] PaginationParams pagination,
        [FromBody] ReviewPostRequestDto body
    )
    {

        // What exactly defines a review ?
        // it can be:
        //     - A REQUEST for a review
        //     - A User wanting to review a project
        //
        //     pub kind: ReviewType,
        //     pub rubric_id: Uuid,
        //     pub user_project_id: Uuid,
        //     pub reviewer_id: Option<Uuid>,
        //
        // What we need to check:
        // - UserProject exists
        // - Rubric exists
        // - User project can be review by anyone
        // - UserProject is in a state that allows reviews: e.g: completed
        // - UserProject has a reviewer assigned to it or if not it is awaiting a reviewer but then it can't be self or auto review
        // - If reviewer_id is specified, we ought to check when is the last time they reviewed a project, they should wait maybe a 8 hours before reviewing the same project again
        // - Can the rubric be used for the user project ?
        // - If the user is a member and asks for a review, we should check if they are allowed to ask for a review only in state of SELF REVIEW can they ask for a self revie

        var userProject = await projectService.FindByIdAsync(body.UserProjectId);
        var rubric = await reviewService.FindByIdAsync(body.RubricId);

        if (userProject == null || rubric == null)
            return UnprocessableEntity(rubric == null ? "Rubric not found" : "User project not found");

        throw new NotImplementedException();

    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/reviews/{id}")]
    public async Task<IActionResult> GetReview(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPatch("/reviews/{id}"), Authorize]
    public async Task<IActionResult> SetReview(Guid id, [FromBody] ReviewPatchRequestDto body)
    {
        throw new NotImplementedException();
    }

    //= /feedback routes =//

    /// <summary>
    ///
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("/feedback"), Authorize]
    public async Task<Feedback> AddFeedback([FromBody] FeebdackPostRequestDto body)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/feedback/{id}")]
    public async Task<Feedback> GetFeedback(
        [FromQuery] PaginationParams pagination,
        [FromBody] CommentPostRequestDto body
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/feedback/{id}/comments")]
    public async Task<IEnumerable<Comment>> GetFeedbackComments(
        [FromQuery] PaginationParams pagination,
        [FromBody] CommentPostRequestDto body
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("/feedback/{id}/comments")]
    public async Task<Comment> AddCommentToFeedback(
        [FromQuery] PaginationParams pagination,
        [FromBody] CommentPostRequestDto body
    )
    {
        throw new NotImplementedException();
    }

    //= /comment routes =//

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/comment"), Authorize]
    public async Task<IEnumerable<Comment>> AddComment(
        [FromQuery] PaginationParams pagination,
        [FromBody] CommentPostRequestDto body
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/comment/{id}"), Authorize]
    public async Task<Comment> AddComment(Guid id)
    {
        throw new NotImplementedException();
    }
}
