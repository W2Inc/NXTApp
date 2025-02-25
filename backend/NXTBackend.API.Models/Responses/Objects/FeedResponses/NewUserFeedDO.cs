using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

public class NewUserFeedDO : FeedDO
{
    public NewUserFeedDO(Feed feed) : base(feed)
    {
        // Additional processing specific to NewUser feed type
        NewUserData = new NewUserData
        {
            JoinDate = feed.CreatedAt,
            DisplayName = feed.Actor?.DisplayName ?? "Unknown User"
        };
    }

    [Required]
    public NewUserData NewUserData { get; set; }
}

public class NewUserData
{
    public DateTimeOffset JoinDate { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}
