
using NXTBackend.API.Models.Requests.Auth;
using NXTBackend.API.Models.Responses.Auth;

namespace NXTBackend.API.Core.Services.Interface;
public interface IAuthService
{
    Task<AuthResponseDto> AuthAsync(AuthRequestDto request);
}