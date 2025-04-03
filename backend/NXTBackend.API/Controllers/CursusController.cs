// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using NXTBackend.API.Core.Graph;
using NXTBackend.API.Core.Graph.V1;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Utils;
using NXTBackend.API.Domain;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using NXTBackend.API.Models.Requests.User;
using NXTBackend.API.Models.Responses.Objects;
using NXTBackend.API.Utils;

// ============================================================================

namespace NXTBackend.API.Controllers;

public class CursusQueryParams
{
    [Description("URL slug to filter on")]
    [FromQuery(Name = "filter[slug]")]
    public string? Slug { get; set; }

    [Description("The entity id to filter on")]
    [FromQuery(Name = "filter[id]")]
    public Guid? Id { get; set; }
}

// ============================================================================

[ApiController]
[Route("cursus"), Authorize]
public class CursusController(
    ILogger<CursusController> logger,
    ICursusService cursusService,
    IUserService userService
) : Controller
{
    [HttpGet("/cursus"), AllowAnonymous]
    [EndpointSummary("Get all exisiting cursi")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CursusDO>>> GetAll(
        [FromQuery] PaginationParams paging,
        [FromQuery] SortingParams sorting,
        [FromQuery] CursusQueryParams filter
    )
    {
        var page = await cursusService.GetAllAsync(paging, sorting);
        page.AppendHeaders(Response.Headers);
        return Ok(page.Items.Select(c => new CursusDO(c)));
    }

    [HttpPost("/cursus")]
    [EndpointSummary("Create a cursus")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CursusDO>> Create([FromBody] CursusPostRequestDTO data)
    {
        var cursus = await cursusService.CreateAsync(new()
        {
            CreatorId = User.GetSID(),
            Markdown = data.Markdown,
            Enabled = data.Enabled,
            Public = data.Public,
            Name = data.Name,
            Slug = data.Name.ToUrlSlug(),
            Description = data.Description,
        });

        return Ok(new CursusDO(cursus));
    }

    [HttpGet("/cursus/{id:guid}"), AllowAnonymous]
    [EndpointSummary("Get a cursus")]
    [EndpointDescription("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CursusDO>> Get(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound();
        return Ok(new CursusDO(cursus));
    }

    [HttpPatch("/cursus/{id:guid}")]
    [EndpointSummary("Update a cursus")]
    [EndpointDescription("Updates a cursus partially based on the provided fields.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CursusDO>> Update(Guid id, [FromBody] CursusPatchRequestDTO data)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound();
        if (User.GetSID() != cursus.CreatorId && !User.IsAdmin())
            return Forbid();

        if (data.Enabled.HasValue)
            cursus.Enabled = data.Enabled.Value;
        if (data.Markdown is not null)
            cursus.Markdown = data.Markdown;
        if (data.Description is not null)
            cursus.Description = data.Description;
        if (data.Name is not null)
        {
            cursus.Name = data.Name;
            cursus.Slug = cursus.Name.ToUrlSlug();
        }

        var updatedCursus = await cursusService.UpdateAsync(cursus);
        return Ok(new CursusDO(updatedCursus));
    }


    [HttpDelete("/cursus/{id:guid}")]
    [EndpointSummary("Delete a cursus")]
    [EndpointDescription("Cursus deletion is rarely done, and only result in deprecations if they have dependencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CursusDO>> Deprecate(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound("Cursus not found");

        await cursusService.DeleteAsync(cursus);
        return Ok(new CursusDO(cursus));
    }

    [HttpPut("/cursus/{id:guid}/path")]
    [EndpointSummary("Set the track / path of a cursus")]
    [EndpointDescription("Sets the actual tree of the cursus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GraphNode>> SetTrack(Guid id, CursusTrackPutRequestDTO data)
    {
        // var cursus = await cursusService.FindByIdAsync(id);
        // if (cursus is null)
        //     return NotFound("Cursus not found");
        // if (cursus.CreatorId != User.GetSID())
        //     return Forbid("You can't edit this cursus");


        var (cursus, user) = await cursusService.IsCollaboratorOnCursus(id, User.GetSID());
        if (cursus is null)
            return NotFound("Cursus not found");

        if (cursus.Track is null)
            return NoContent();
        return Ok(cursus.Track);
    }

    [HttpGet("/cursus/{id:guid}/path")]
    [EndpointSummary("Get the track / path of a cursus")]
    [EndpointDescription("Lets you retrieve the binary data of the track ")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GraphNode>> GetTrack(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound("Cursus not found");
        if (cursus.Track is null)
            return NoContent();
        return Ok(cursus.Track);
    }
}
