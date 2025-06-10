// ============================================================================
// Copyright (c) 2025 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Models.Responses.Objects;

namespace NXTBackend.API.Controllers;

[ApiController]
[Tags("Hooks")]
[Route("webhooks/git")]
[AllowAnonymous]
#if DEBUG
[ApiExplorerSettings(IgnoreApi = false)]
#else
[ApiExplorerSettings(IgnoreApi = true)]
#endif
public class GitHookController(ILogger<NotificationController> logger) : Controller
{
    [HttpPost("/webhooks/git")]
    [EndpointSummary("Create a notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDO>> OnRepoCreate()
    {
        Request.Headers.TryGetValue("X-Gitea-Event", out var eventType);
        Request.Headers.TryGetValue("X-Gitea-Signature", out var signature);

        if (string.IsNullOrEmpty(eventType))
            return BadRequest(new ProblemDetails
            {
                Title = "Missing Event Type",
                Detail = "X-Gitea-Event header is required."
            });

        // Read the request body to get the webhook payload
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(payload))
            return BadRequest(new ProblemDetails
            {
                Title = "Empty Payload",
                Detail = "Webhook payload cannot be empty."
            });

        // Log the received event
        logger.LogInformation("Received Gitea webhook event: {EventType}", eventType);

        // TODO: Verify webhook signature if webhook secret is configured
        // TODO: Process the webhook payload based on event type

        return Ok();
    }
}