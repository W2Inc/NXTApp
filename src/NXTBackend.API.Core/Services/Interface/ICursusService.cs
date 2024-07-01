namespace NXTBackend.API.Core.Services.Interface;

using NXTBackend.API.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Requests;

public interface ICursusService : IDomainService<Cursus>
{

    /// <summary>
    /// Find a goal in the cursus
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    Task<LearningGoal?> FindGoal(Cursus cursus, LearningGoal goal);

    /// <summary>
    /// Get all the projects that are part of a learning goal.
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<PaginatedList<LearningGoal>> GetGoals(Cursus cursus, PaginationParams pagination);
}