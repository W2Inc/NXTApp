using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Models.Responses.Objects.FeedResponses;

public class NewUserFeedDO(Feed feed, BaseEntity user) : FeedDO(feed)
{
    [Required]
    public MinimalUserDTO NewUser { get; set; } = (User)user!;
}
