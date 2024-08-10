using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Graph;
using NXTBackend.API.Core.Graph.Meta;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using Serilog;

namespace NXTBackend.API.Controllers;

[Route("cursus")]
[ApiController]
public class CursusController(
    ICursusService cursusService,
    IUserService userService
) : ControllerBase
{

    /// <summary>
    /// Get all comments
    /// </summary>
    /// <returns>All current existing events</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesResponseType<Event>(200)]
    [ProducesResponseType<ErrorResponseDto>(400)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    [HttpGet("/cursus")]
    public async Task<IActionResult> GetCursi([FromQuery] PaginationParams pagination)
    {
        var list = await cursusService.GetAllAsync(pagination);
        var headers = list.GetHeaders();
        foreach (var header in headers)
            HttpContext.Response.Headers.Append(header.Key, header.Value);
        return Ok(list.Items);
    }

    [HttpPost("/cursus")]
    public async Task<IActionResult> CreateCursus([FromBody] CursusPostRequestDto request)
    {
        if (await cursusService.FindByNameAsync(request.Name) is not null)
            return Conflict(new ErrorResponseDto("Cursus already exists"));

        var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ErrorResponseDto("User not found"));

        var user = await userService.FindByIdAsync(Guid.Parse(userId.Value));
        if (user is null)
            return Unauthorized(new ErrorResponseDto("User not found"));

        var cursus = await cursusService.CreateAsync(new()
        {
            Name = request.Name,
            Description = request.Description,
            Markdown = request.Markdown,
            CreatorId = user.Id,
            Kind = request.Kind,
            Public = request.Public,
            Enabled = request.Enabled,
            Slug = request.Name.ToLower().Replace(" ", "-"),
        });

        return Ok(cursus);
    }

    [HttpGet("/cursus/{id}")]
    public async Task<IActionResult> GetCursus(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound(new ErrorResponseDto("Cursus not found"));
        return Ok(cursus);
    }

    [HttpPatch("/cursus/{id}")]
    public async Task<IActionResult> UpdateCursus(Guid id, [FromBody] CursusPatchRequestDto request)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound(new ErrorResponseDto("Cursus not found"));

        cursus.Name = request.Name;
        cursus.Description = request.Description;
        await cursusService.UpdateAsync(cursus);
        return Ok(cursus);
    }

    [HttpDelete("/cursus/{id}")]
    public async Task<IActionResult> DeleteCursus(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound(new ErrorResponseDto("Cursus not found"));

        await cursusService.DeleteAsync(cursus);
        return Ok();
    }


    [HttpPut("/cursus/{id}/path")]
    public async Task<IActionResult> AddCursi(Guid id)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus is null)
            return NotFound(new ErrorResponseDto("Cursus not found"));

        using var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);

        memoryStream.Position = 0;
        using (var reader = new GraphReader(memoryStream))
        {
            reader.ReadData();
            //reader.RootNode;
            // TODO(W2): Verify the data inside the graph
            // - Do goals exist...
            // - etc ...
        }

        cursus.Track = memoryStream.ToArray();
        await cursusService.UpdateAsync(cursus);
        Log.Information("Cursus added");
        return Ok();
    }

    /// <summary>
    /// Get the track data for a specific cursus
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    /// <response code="429">Too many requests</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">An Internal server error has occurred</response>
    [ProducesErrorResponseType(typeof(ErrorResponseDto))]
    [ProducesResponseType<FileStreamResult>(200, "application/octet-stream")]
    [HttpGet("/cursus/{id}/path")]
    public async Task<IActionResult> GetPath(Guid id, [FromQuery] PaginationParams format)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus == null)
            return NotFound(new ErrorResponseDto("Cursus not found"));

        try
        {
            var memoryStream = new MemoryStream(cursus.Track);
            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, "application/octet-stream")
            {
                FileDownloadName = $"{id}.graph"
            };
        }
        catch (InvalidDataException e)
        {
            return BadRequest(new ErrorResponseDto(e.Message));
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to read graph data");
            return StatusCode(500, new ErrorResponseDto("Failed to read graph data"));
        }
    }
}

//public class GraphGenerator
//{
//    private ushort currentId = 0;

//    public GraphNode GenerateFakeData(int levels, int nodesPerLevel)
//    {
//        return GenerateNode(0, levels, nodesPerLevel, "Root");
//    }

//    private GraphNode? GenerateNode(ushort parentId, int levelsRemaining, int nodesPerLevel, string goalName)
//    {
//        if (levelsRemaining <= 0)
//        {
//            return null;
//        }

//        var node = new GraphNode
//        {
//            Id = currentId++,
//            ParentId = parentId,
//        };

//        // Generate random amount of goals
//        int amount = new Random().Next(1, 3);
//        for (int i = 0; i < amount; i++)
//        {
//            node.Goals.Add(new GoalEntry
//            {
//                Name = $"Goal {i}",
//                GoalId = Guid.NewGuid()
//            });
//        }

//        for (int i = 0; i < nodesPerLevel; i++)
//        {
//            var childNode = GenerateNode(node.Id, levelsRemaining - 1, nodesPerLevel, $"Goal {currentId}");
//            if (childNode != null)
//            {
//                node.Children.Add(childNode);
//            }
//        }

//        return node;
//    }
//}
