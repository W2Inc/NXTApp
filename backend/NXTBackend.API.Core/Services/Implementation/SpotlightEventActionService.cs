using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Spotlight;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class SpotlightEventActionService(DatabaseContext ctx) : BaseService<SpotlightEventAction>(ctx), ISpotlightEventActionService
{

}
