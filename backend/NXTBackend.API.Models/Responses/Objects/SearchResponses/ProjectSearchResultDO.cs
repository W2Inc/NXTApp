using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Models.Responses.Objects.SearchResponses;

public class ProjectSearchResultDO(Project project) : SearchResultDO()
{
    [Required]
    public ProjectDO Project { get; set; } = project!;
}
