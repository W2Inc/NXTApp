using System.ComponentModel;

namespace NXTBackend.API.Models;


/// <summary>
/// To avoid usage of reflection we define this record of shared/common properties to filter.
/// </summary>
/// <remarks>
/// The flow for adding a new query is as follows:
/// - I want to filter on a new property
/// - I add my query param here
/// - I go to the service(s) that can filter on this new query
/// - In the `ApplyFilters` I append the way it should filter with this new property
/// - In the controller I ensure my controller specific filter has the property and passes it
/// on to the service function.
/// </remarks>
/// <param name="Id"> Filter on ID </param>
/// <param name="Slug"> Filter on slug </param>
public record QueryFilters
{
    public Guid? Id { get; init; }
    public string? Slug { get; init; }
};
