using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class CursusService(DatabaseContext ctx) : BaseService<Cursus>(ctx), ICursusService
{
    public Task<Cursus?> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}
