
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Core.Services.Interface;
public interface ICursusService : IDomainService<Cursus>
{
    public Task<Cursus?> FindByNameAsync(string name);
}
