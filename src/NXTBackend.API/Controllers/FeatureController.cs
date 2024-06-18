using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Common;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Requests.Auth;

namespace NXTBackend.API.Controllers;

[Route("features")]
[ApiController]
public class FeatureController(IUserService userService) : ControllerBase
{
    private readonly IUserService _searchService = userService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/feature")]
    public async Task<IEnumerable<Feature>> GetFeatures([FromQuery] PaginationParams pagination)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("/feature/{id}")]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPatch("/feature/{id}"), Authorize("dev")]
    public async Task<IActionResult> SetFeature(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("/feature/{id}"), Authorize("dev")]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        throw new NotImplementedException();
    }
}
