// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NXTBackend.API.Core.Notifications;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
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
            .AddFilter("user_id", User.GetSID());

        var page = await notificationService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(item => new NotificationDO(item)));
    }

    [HttpPost("/notifications"), /* Authorize(Roles = "admin") */]
    [EndpointSummary("Create a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Create([FromBody] NotificationPostDTO data)
    {
        var user = await userService
			.Include(u => u.Details)
			.FindByIdAsync(data.UserId);

        if (user is null)
			return NotFound();
		notifications.Enqueue(user, new Welcome(user));
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
