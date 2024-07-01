
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Interface;
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