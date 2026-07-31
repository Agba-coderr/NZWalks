using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityUser?> RegisterAsync(RegisterRequestDto registerRequestDto);

        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
