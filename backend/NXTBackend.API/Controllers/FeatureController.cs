// ============================================================================
// Nextdemy B.V, Amsterdam 2023, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests.Feature;
using NXTBackend.API.Models.Responses.Objects;

// ============================================================================

namespace NXTBackend.API.Controllers;

[ApiController, Route("features")]
public class FeatureController(IFeatureService featureService) : ControllerBase
{
    /// <summary>
    /// Get all features
    /// </summary>
    /// <param name="pagination">The pagination parameters</param>
    /// <param name="sorting">The sorting parameters</param>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="429">Too many requests</response>
    [HttpGet("/features")]
    [ProducesResponseType<FeatureDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeatures([FromQuery] PaginationParams pagination, [FromQuery] SortingParams sorting)
    {
        var list = await featureService.GetAllAsync(pagination, sorting);
        list.AppendHeaders(Response.Headers);
        return Ok(list.Items);
    }

    /// <summary>
    /// Get a specific feature
    /// </summary>
    /// <param name="id">The feature ID</param>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Feature by that name exists</response>
    [HttpGet("/features/{id}")]
    [ProducesResponseType<FeatureDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        var feature = await featureService.FindByIdAsync(id);
        return feature is null ? NotFound() : Ok(new FeatureDO(feature));
    }

    /// <summary>
    /// Creates a new feature.
    /// </summary>
    /// <param name="body">The DTO to post with.</param>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Feature by that name exists</response>
    [HttpPost("/features"), Authorize("admin")]
    [ProducesResponseType<FeatureDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetFeature([FromBody] FeaturePostRequestDTO body)
    {
        return await featureService.FindByNameAsync(body.Name) is not null
            ? Conflict("Feature by that name exists")
            : Ok(await featureService.CreateAsync(new Feature
            {
                Name = body.Name,
                Markdown = body.Markdown,
                IsPublic = body.Public
            }));
    }

    /// <summary>
    /// Updates a new feature.
    /// </summary>
    /// <param name="id">The feature to update.</param>
    /// <param name="body">The DTO to post with.</param>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    [HttpPatch("/features/{id}"), Authorize("admin")]
    [ProducesResponseType<FeatureDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetFeature(Guid id, [FromBody] FeaturePatchRequestDTO body)
    {
        var feature = await featureService.FindByIdAsync(id);
        if (feature is null)
            return NotFound();

        feature.Name = body.Name ?? feature.Name;
        feature.Markdown = body.Markdown ?? feature.Markdown;
        feature.IsPublic = body.Public ?? feature.IsPublic;
        return Ok(await featureService.UpdateAsync(feature));
    }

    /// <summary>
    /// Delete a feature.
    /// </summary>
    /// <param name="id">The feature to delete.</param>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not found</response>
    [HttpDelete("/features/{id}"), Authorize("admin")]
    [ProducesResponseType<FeatureDO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        var feature = await featureService.FindByIdAsync(id);
        return feature is null ? NotFound() : Ok(await featureService.DeleteAsync(feature));
    }
}
