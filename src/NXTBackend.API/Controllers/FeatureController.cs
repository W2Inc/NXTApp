using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Feature;
using NXTBackend.API.Models.Responses;
using Serilog;

namespace NXTBackend.API.Controllers;

[Route("features")]
[ApiController]
public class FeatureController(IFeatureService featureService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="filters"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    [HttpGet("/features")]
    [ProducesResponseType<IEnumerable<Feature>>(200)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    public async Task<IActionResult> GetFeatures(
        [FromQuery] PaginationParams pagination
    )
    {
        try
        {
            var list = await featureService.GetAllAsync(pagination);
            HttpContext.Response.Headers.Append("X-Next-Page", list.HasNextPage.ToString());
            HttpContext.Response.Headers.Append("X-Prev-Page", list.HasPreviousPage.ToString());
            HttpContext.Response.Headers.Append("X-Page", list.Page.ToString());
            HttpContext.Response.Headers.Append("X-Count", list.TotalCount.ToString());
            HttpContext.Response.Headers.Append("X-Pages", list.TotalPages.ToString());
            return Ok(list.Items);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while fetching features");
            return StatusCode(500, new ErrorResponseDto());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Returns the feature</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("/features/{id}")]
    [ProducesResponseType<Feature>(200)]
    [ProducesResponseType<ErrorResponseDto>(404)]
    [ProducesResponseType<ErrorResponseDto>(500)]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        var feature = await featureService.FindByIdAsync(id);
        if (feature is null)
            return NotFound(new ErrorResponseDto("Feature not found"));

        return Ok(new FeatureResponseDto()
        {
            Name = feature.Name,
            IsPublic = feature.IsPublic,
            Markdown = feature.Markdown,
        });
    }

    /// <summary>
    /// Update a feature
    /// </summary>
    /// <param name="id"> The id of the feature to update</param>
    /// <returns> The updated feature</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="500">An Internal server error has occurred</response>
    //[Authorize("dev")]
    [HttpPost("/features")]
    public async Task<IActionResult> SetFeature(Guid id, [FromBody] FeaturePostRequestDto body)
    {
        var feature = await featureService.FindByNameAsync(body.Name);
        if (feature is not null)
        {
            return Conflict("blah"); // Response in text plain ? WHat do I use for a uniform error ??
        }

        return Ok(await featureService.CreateAsync(new Feature
        {
            Name = body.Name,
            IsPublic = body.Public,
            Markdown = body.Markdown
        }));
    }

    /// <summary>
    /// Update a feature
    /// </summary>
    /// <param name="id"> The id of the feature to update</param>
    /// <returns> The updated feature</returns>
    /// <response code="200">The updated feature</response>
    /// <response code="500">An Internal server error has occurred</response>
    //[Authorize("dev")]
    [HttpPatch("/features/{id}")]
    public async Task<IActionResult> SetFeature(Guid id, [FromBody] FeaturePatchRequestDto body)
    {
        var feature = await featureService.FindByIdAsync(id);
        if (feature is null)
            return NotFound();

        // Coalesce the values
        feature.Name = body.Name ?? feature.Name;
        feature.IsPublic = body.Public ?? feature.IsPublic;
        feature.Markdown = body.Markdown ?? feature.Markdown;
        return Ok(await featureService.UpdateAsync(feature));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //[Authorize("dev")]
    [HttpDelete("/features/{id}")]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        var feature = await featureService.FindByIdAsync(id);
        if (feature is null)
            return NotFound();
        return Ok(await featureService.DeleteAsync(feature));
    }
}
