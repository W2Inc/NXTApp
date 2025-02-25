using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

public class CompletedProjectFeedDO : FeedDO
{
    public CompletedProjectFeedDO(Feed feed) : base(feed)
    {
        // Here you would typically fetch project details using the ResourceId
        ProjectData = new ProjectCompletionData
        {
            ProjectId = feed.ResourceId ?? Guid.Empty,
            CompletedAt = feed.CreatedAt,
            ProjectName = "Project Name" // In real code, you'd fetch this from your DB
        };
    }

    [Required]
    public ProjectCompletionData ProjectData { get; set; }
}

public class ProjectCompletionData
{
    public Guid ProjectId { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public string ProjectName { get; set; } = string.Empty;
}
