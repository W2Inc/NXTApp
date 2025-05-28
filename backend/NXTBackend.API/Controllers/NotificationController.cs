// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Notifications;
using NXTBackend.API.Core.Notifications.Variants;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

namespace NXTBackend.API.Controllers;

[ApiController]
[Route("notifications"), Authorize]
public class NotificationController(
    ILogger<NotificationController> logger,
    INotificationService notificationService,
	INotificationQueue notifications,
    IUserService userService
) : Controller
{
    [HttpGet("/notifications"), OutputCache(PolicyName = "1m")]
    [EndpointSummary("Get all notifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<NotificationDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[id]")] Guid? id,
        [FromQuery(Name = "filter[read]")] bool? read
    )
    {
        var filters = new FilterDictionary()
			.AddFilter(nameof(id), id)
			.AddFilter(nameof(read), read)
			.AddFilter("not[kind]", NotificationKind.FeedOnly)
            .AddFilter("user_id", User.GetSID());

        var page = await notificationService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(item => new NotificationDO(item)));
    }

    [HttpPost("/notifications"), /* Authorize(Roles = "admin") */]
    [EndpointSummary("Create a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Create([FromBody] NotificationPostDTO data, IUserProjectService userProjectService, IReviewService reviewService)
    {

        var review = await reviewService.FindByIdAsync(new Guid("01966da1-6e6f-738b-96bf-7dc64970881f"));
        var review2 = await reviewService.Query()
            .Include(r => r.UserProject.Project)
            .FirstOrDefaultAsync();

        var user = await userService.FindByIdAsync(User.GetSID());
        notifications.Enqueue(user, new ReviewNotification(user, review2));

  //       var user = await userService.Query(false)
        //           .Include(u => u.Details)
        // 	.FirstOrDefaultAsync();
        //
        //       if (user is null)
        // 	return NotFound();
        // // notifications.Enqueue(user, new Welcome(user));
        //
        // notifications.Enqueue(user, new WelcomeNotification(user));
        // var up = await userProjectService
        // 	.Include(up => up.Project)
        // 	.Include(up => up.Members)
        // 	.FindByIdAsync(new Guid("01966da1-6e6f-738b-96bf-7dc64970881f"));
        // notifications.Enqueue(user, new InviteNotification(user, up));
        // // var review = await reviewService.CreateAsync(new()
        // // {
        // // 	UserProjectId = up.Id,
        // // 	ReviewerId = user.Id,
        // // 	State = ReviewState.InProgress,
        // // });
        //
        // var review = await reviewService
        // 	.Include(r => r.UserProject)
        // 	.FindByIdAsync(new Guid("01970df7-6f3a-7bad-8543-195dfd5cabf7"));
        //
        // if (review is null)
        // 	return BadRequest("Failed to create review");
        //
        // // Enqueue the review notification
        //
        // notifications.Enqueue(user, new ReviewNotification(user, review));
        return Ok();
    }

    [HttpPost("/notifications/read")]
    [EndpointSummary("Mark notifications as read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsRead([FromBody] IEnumerable<Guid>? notificationIds)
    {
        if (notificationIds is not null && !await notificationService.AreValid(notificationIds))
            return BadRequest("Request contains invalid Notification IDs");

        await notificationService.MarkAsReadAsync(User.GetSID(), notificationIds);
        return Ok();
    }

    [HttpPost("/notifications/unread")]
    [EndpointSummary("Mark notifications as unread")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAUnRead([FromBody] IEnumerable<Guid>? notificationIds)
    {
        if (notificationIds is not null && !await notificationService.AreValid(notificationIds))
            return BadRequest("Request contains invalid Notification IDs");

        await notificationService.MarkAsUnreadAsync(User.GetSID(), notificationIds);
        return Ok();
    }

    [HttpDelete("/notifications/{id:guid}"), Authorize(Roles = "admin")]
    [EndpointSummary("Delete a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Delete(Guid id)
    {
        var entity = await notificationService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();

        await notificationService.DeleteAsync(entity);
        return Ok(new NotificationDO(entity));
    }
}
