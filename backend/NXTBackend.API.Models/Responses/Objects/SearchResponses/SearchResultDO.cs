// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Domain.Enums;
using System.Text.Json.Serialization;

namespace NXTBackend.API.Models.Responses.Objects.SearchResponses;

[JsonDerivedType(typeof(UserSearchResultDO), typeDiscriminator: nameof(SearchKind.User))]
[JsonDerivedType(typeof(ProjectSearchResultDO), typeDiscriminator: nameof(SearchKind.Project))]
public abstract class SearchResultDO()
{

}
