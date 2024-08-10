
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Models;
using NXTBackend.API.Models.Requests;
using NXTBackend.API.Models.Responses;

namespace NXTBackend.API.Core.Services.Interface;

public interface ISearchService
{
    /// <summary>
    /// Search for a specific user.
    /// </summary>
    /// <param name="query">The query to use for searching.</param>
    /// <returns>The search result of regarding the user</returns>
    Task<SearchResponseDto<User>> SearchUserAsync(SearchRequestDto request, PaginationParams pagination);

    /// <summary>
    /// Search for a specific project.
    /// </summary>
    /// <param name="query">The query to use for searching.</param>
    /// <returns>The search result of regarding the project</returns>
    Task<SearchResponseDto<Project>> SearchProjectAsync(SearchRequestDto request, PaginationParams pagination);

    /// <summary>
    /// Search for a specific cursus.
    /// </summary>
    /// <param name="query">The query to use for searching.</param>
    /// <returns>The search result of regarding the cursus</returns>
    Task<SearchResponseDto<Cursus>> SearchCursusAsync(SearchRequestDto request, PaginationParams pagination);

    /// <summary>
    /// Search for a specific learning goal.
    /// </summary>
    /// <param name="query">The query to use for searching.</param>
    /// <returns>The search result of regarding the learning goal</returns>
    Task<SearchResponseDto<LearningGoal>> SearchGoalAsync(SearchRequestDto request, PaginationParams pagination);
}