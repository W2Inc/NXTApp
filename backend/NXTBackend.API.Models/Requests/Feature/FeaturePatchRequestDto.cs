using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Requests.Feature;

public class FeaturePatchRequestDTO : BaseRequestDTO
{
    /// <summary>
    /// The name of the feature
    /// </summary>
    [StringLength(128, MinimumLength = 4)]
    public string? Name { get; set; }

    /// <summary>
    /// The markdown content of the feature
    /// </summary>
    [StringLength(2048, MinimumLength = 128)]
    public string? Markdown { get; set; }

    /// <summary>
    /// Is the feature public / visible?
    /// </summary>
    public bool? Public { get; set; }
}
