namespace NXTBackend.API.Core.Services.Interface;

using NXTBackend.API.Models.Requests.Auth;
using NXTBackend.API.Models.Responses.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> AuthAsync(AuthRequestDto request);
}