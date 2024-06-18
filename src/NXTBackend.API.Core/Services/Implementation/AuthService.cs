using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Requests.Auth;
using NXTBackend.API.Models.Responses.Auth;

namespace NXTBackend.API.Core.Services.Implementation;

public sealed class AuthService : IAuthService
{
    private readonly DatabaseContext _databaseContext;

    public AuthService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<AuthResponseDto> AuthAsync(AuthRequestDto request)
    {
        await Task.Delay(2000);

        var features = await _databaseContext.Features.Select(x => x.Name).ToListAsync();

        return new()
        {
            AccessToken = string.Join(",", features),
            Success = request.Password == "weak"
        };
    }

}