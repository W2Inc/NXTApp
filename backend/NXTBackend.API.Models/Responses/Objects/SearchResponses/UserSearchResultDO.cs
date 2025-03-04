using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Models.Responses.Objects.SearchResponses;

public class UserSearchResultDO(User user) : SearchResultDO()
{
    [Required]
    public MinimalUserDTO user { get; set; } = user!;
}
