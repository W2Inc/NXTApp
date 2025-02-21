// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Domain.Services;
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
    [HttpGet("/notifications")]
    [EndpointSummary("Get all notifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<NotificationDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery(Name = "filter[id]")] Guid? id
    )
    {
        var filters = new FilterDictionary()
            .AddFilter("id", id);

        var page = await notificationService.GetAllAsync(paging, sorting, filters);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(item => new NotificationDO(item)));
    }

    [HttpPost("/notifications")]
    [EndpointSummary("Create a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Create([FromBody] NotificationPostDTO data)
    {
        var entity = await notificationService.CreateAsync(new()
        {
            Message = data.Message,
            Kind = data.Kind,
            ResourceId = data.ResourceId ?? null
        });

        return Ok(new NotificationDO(entity));
    }

    [HttpGet("/notifications/{id:guid}")]
    [EndpointSummary("Get a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Get(Guid id)
    {
        var entity = await notificationService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();
        return Ok(new NotificationDO(entity));
    }

    [HttpPatch("/notifications/{id:guid}")]
    [EndpointSummary("Update a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> Update(Guid id, [FromBody] NotificationPatchDTO data)
    {
        var entity = await notificationService.FindByIdAsync(id);
        if (entity is null)
            return NotFound();
        if (!User.IsAdmin())
            return Forbid();

        var updatedNotification = await notificationService.UpdateAsync(entity);
        return Ok(new NotificationDO(updatedNotification));
    }

    [HttpDelete("/notifications/{id:guid}")]
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
