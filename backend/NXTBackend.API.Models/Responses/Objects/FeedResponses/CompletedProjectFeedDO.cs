using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

public class CompletedProjectFeedDO(Feed feed, BaseEntity project) : FeedDO(feed)
{
    [Required]
    public ProjectDO NewProject { get; set; } = (Project)project!;
}
