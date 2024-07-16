using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Graph;
using NXTBackend.API.Core.Graph.Meta;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Cursus;
using Serilog;

namespace NXTBackend.API.Controllers;

[Route("cursus")]
[ApiController]
public class CursusController(ICursusService cursusService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus")]
    public async Task<IActionResult> GetCursi([FromQuery] PaginationParams pagination)
    {
        var list = await cursusService.GetAllAsync(pagination);
        var headers = list.GetHeaders();
        foreach (var header in headers)
            HttpContext.Response.Headers.Append(header.Key, header.Value);
        return Ok(list.Items);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpPut("/cursus/{id}/path"), Authorize]
    public async Task<IActionResult> AddCursi(Guid id)
    {

        using var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);

        memoryStream.Position = 0;
        using (var reader = new GraphReader(memoryStream))
        {
            reader.ReadHeader();
            reader.ReadData();
        }

        Log.Information("Cursus added");
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus/{id}")]
    public async Task<IEnumerable<User>> GetCursus([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpPatch("/cursus/{id}"), Authorize]
    public async Task<IEnumerable<User>> SetCursus([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("/cursus/{id}/path")]
    [ProducesResponseType<FileStreamResult>(200, "application/octet-stream")]
    public async Task<IActionResult> GetPath(Guid id, [FromQuery] PaginationParams pagination)
    {
        var cursus = await cursusService.FindByIdAsync(id);
        if (cursus == null)
            return NotFound(new ErrorResponseDto("Cursus not found"));

        var nodes = new List<GraphNode>()
        {
            new GraphNode()
            {
                Id = 0,
                ParentId = 0,
                Goals = new List<GoalEntry>
                {
                    new GoalEntry
                    {
                        Name = $"Goal 1",
                        GoalId = Guid.NewGuid()
                    },
                    new GoalEntry
                    {
                        Name = $"Goal 2",
                        GoalId = Guid.NewGuid()
                    }
                },
                Next = new List<GraphNode>
                {
                    new GraphNode()
                    {
                        Id = 1,
                        ParentId = 0,
                        Goals = new List<GoalEntry>
                        {
                            new GoalEntry
                            {
                                Name = $"Goal 3",
                                GoalId = Guid.NewGuid()
                            },
                            new GoalEntry
                            {
                                Name = $"Goal 4",
                                GoalId = Guid.NewGuid()
                            }
                        }
                    }
                }
            },
        };

        var memoryStream = new MemoryStream();
        using (var writer = new GraphWriter(memoryStream))
            writer.WriteData(nodes);

        memoryStream.Position = 0; // Reset the memory stream position
        return new FileStreamResult(memoryStream, "application/octet-stream")
        {
            FileDownloadName = $"{id}.graph"
        };
    }
}
