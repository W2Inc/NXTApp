
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Core.Utils.Query;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface;
public interface IGoalService : IDomainService<LearningGoal>, ICollaborative<LearningGoal>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="pagination"></param>
    /// <param name="sorting"></param>
    /// <returns></returns>
    Task<PaginatedList<User>> GetUsers(LearningGoal goal, PaginationParams pagination, SortingParams sorting);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="pagination"></param>
    /// <param name="sorting"></param>
    /// <returns></returns>
    Task<PaginatedList<Cursus>> GetCursi(LearningGoal goal, PaginationParams pagination, SortingParams sorting);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="sorting"></param>
    /// <returns></returns>
    Task<IEnumerable<Project>> GetProjects(LearningGoal goal, SortingParams sorting);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="projects"></param>
    /// <returns></returns>
    Task<IEnumerable<Project>> SetProjects(LearningGoal goal, IEnumerable<Guid> projects);
}
