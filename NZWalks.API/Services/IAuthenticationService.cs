using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IAuthenticationService
    {
        // Return IdentityResult so callers can surface specific Identity errors
        Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto);

        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
