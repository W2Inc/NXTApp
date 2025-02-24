// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NXTBackend.API.Core.Services.Interface;
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
    INotificationService notificationService
) : Controller
{
    // [HttpGet("/notifications"), OutputCache(PolicyName = "1m")]
    // [EndpointSummary("Get all notifications")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<IEnumerable<NotificationDO>>> GetAll(
    //     [FromQuery] PaginationParams paging,
    //     [FromQuery] SortingParams sorting,
    //     [FromQuery(Name = "filter[id]")] Guid? id
    // )
    // {
    //     var filters = new FilterDictionary()
    //         .AddFilter(nameof(id), id)
    //         // Notifications are by default, private, as such we can only give back system wide ones.
    //         .AddFilter("type", NotificationKind.System);

    //     var page = await notificationService.GetAllAsync(paging, sorting, filters);
    //     page.AppendHeaders(Response.Headers);
    //     return Ok(page.Items.Select(item => new NotificationDO(item)));
    // }

    [HttpPost("/notifications"), /* Authorize(Roles = "admin") */]
    [EndpointSummary("Create a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Create([FromBody] NotificationPostDTO data)
    {
        var entity = await notificationService.SendNotificationRangeAsync([data.UserId], data.Message, data.Kind);
        return Ok(new NotificationDO(entity));
    }

    // [HttpPost("/notifications/read")]
    // [EndpointSummary("Mark notifications as read")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> MarkAsRead([FromBody] IEnumerable<Guid> notificationIds)
    // {
    //     // Validate that the notifications exist and user has access to them
    //     var notifications = await notificationService.FindByIdsAsync(notificationIds);
    //     var invalidIds = notificationIds.Except(notifications.Select(n => n.Id));

    //     if (invalidIds.Any())
    //     {
    //         return BadRequest($"Invalid notification IDs: {string.Join(", ", invalidIds)}");
    //     }

    //     // Check if user has access to these notifications
    //     var userId = User.GetSID();
    //     var unauthorized = notifications.Where(n =>
    //         n.Kind != NotificationKind.System &&
    //         !n.UserNotifications.Any(un => un.UserId == userId));

    //     if (unauthorized.Any())
    //     {
    //         return Forbid();
    //     }

    //     await notificationService.MarkAsReadAsync(userId, notificationIds);
    //     return Ok();
    // }

    // [HttpGet("/notifications/{id:guid}")]
    // [EndpointSummary("Get a notification")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<NotificationDO>> Get(Guid id)
    // {
    //     var entity = await notificationService.FindByIdAsync(id);
    //     if (entity is null)
    //         return NotFound();
    //     return Ok(new NotificationDO(entity));
    // }

    // [HttpPatch("/notifications/{id:guid}"), Authorize(Roles = "admin")]
    // [EndpointSummary("Update a notification")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<NotificationDO>> Update(Guid id, [FromBody] NotificationPatchDTO data)
    // {
    //     var entity = await notificationService.FindByIdAsync(id);
    //     if (entity is null)
    //         return NotFound();
    //     if (!User.IsAdmin())
    //         return Forbid();

    //     var updatedNotification = await notificationService.UpdateAsync(entity);
    //     return Ok(new NotificationDO(updatedNotification));
    // }

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
