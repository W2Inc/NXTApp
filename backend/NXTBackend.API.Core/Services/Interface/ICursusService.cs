
using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface;
public interface ICursusService : ICollaborative<Cursus>, IDomainService<Cursus>
{
    Task<Cursus?> FindByNameAsync(string name);
}
