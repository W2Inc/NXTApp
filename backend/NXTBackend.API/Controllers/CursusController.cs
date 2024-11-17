// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using NXTBackend.API.Core.Graph;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

[Route("cursus")]
[ApiController]
public class CursusController(
    ILogger<CursusController> logger,
    ICursusService cursusService,
    IUserService userService
) : Controller
{
    [HttpGet("/cursus")]
    [EndpointSummary("Get all exisiting cursi")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CursusDO>>> GetAll([FromQuery] PaginationParams paging, [FromQuery] SortingParams sorting)
    {
        var page = await cursusService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new CursusDO(c)));
    }

    [HttpPost("/cursus"), Authorize]
    [EndpointSummary("Create a cursus")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK),]
    public async Task<ActionResult<CursusDO>> Create([FromBody] CursusPostRequestDTO data)
    {
        var cursus = cursusService.CreateAsync(new()
        {
            CreatorId = User.GetSID(),
            Markdown = data.Markdown,
            Enabled = data.Enabled,
            Description = data.Description,

        });

        return Ok(cursus);
    }

    [HttpGet("/cursus/{id}")]
    [EndpointSummary("Get a cursus")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CursusDO>> Get(Guid id)
    {
        return Ok(await cursusService.FindByIdAsync(id));
    }

    [HttpPatch("/cursus/{id}")]
    [EndpointSummary("Update a cursus")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CursusDO>> Update(Guid id, [FromBody] CursusPatchRequestDTO data)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound();

        if (User.GetSID() != cursus.CreatorId && User.IsAdmin())
            return Forbid();

        // TODO: Bad, it updates EVERYTHING!
        cursus.Enabled = data.Enabled ?? cursus.Enabled;
        cursus.Markdown = data.Markdown ?? cursus.Markdown;
        cursus.Description = data.Description ?? cursus.Description;
        if (data.Name is not null)
        {
            cursus.Name = data.Name ?? cursus.Name;
            cursus.Slug = cursus.Name.ToUrlSlug();
        }

        return Ok(new CursusDO(await cursusService.UpdateAsync(cursus)));
    }

    [HttpDelete("/cursus/{id}")]
    [EndpointSummary("Delete a cursus")]
    [EndpointDescription("Cursus deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CursusDO>> Deprecate(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound("Cursus not found");

        cursus.Enabled = false;
        await cursusService.UpdateAsync(cursus);
        return Ok(cursus);
    }

    [HttpPut("/cursus/{id}/path")]
    [Consumes("application/octet-stream")]
    [EndpointSummary("Define the track / path of a cursus")]
    [EndpointDescription("Cursi can have a set track of goals ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CursusDO>> SetTrack(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound("Cursus not found");

        using var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        try
        {
            using var reader = new GraphReader(memoryStream);
            reader.ReadData();
            //reader.RootNode;
            // TODO(W2): Verify the data inside the graph
            // - Do goals exist...
            // - etc ...
        }
        catch (InvalidDataException e)
        {
            return UnprocessableEntity(new ProblemDetails()
            {
                Title = e.Message
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to read graph data");
            return Problem(e.Message);
        }

        cursus.Track = memoryStream.ToArray();
        await cursusService.UpdateAsync(cursus);
        logger.LogInformation("Cursus added");
        return Ok();
    }

    [HttpGet("/cursus/{id}/path")]
    [Produces(contentType: "application/octet-stream")]
    public async Task<IActionResult> GetPath(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound("Cursus not found");

        try
        {
            var memoryStream = new MemoryStream(cursus.Track)
            {
                Position = 0
            };

            return new FileStreamResult(memoryStream, "application/octet-stream")
            {
                FileDownloadName = $"{id}.graph"
            };
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to read graph data");
            return Problem(e.Message);
        }
    }
}
