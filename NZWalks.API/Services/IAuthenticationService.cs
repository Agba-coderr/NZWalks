using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.Common;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IAuthenticationService
    {
        // Return IdentityResult so callers can surface specific Identity errors
        Task<Result> RegisterAsync(RegisterRequestDto registerRequestDto);

        Task<Result> VerifyEmailAsync(string userId, string token);

        Task<Result> LoginAsync(LoginRequestDto loginRequestDto);

        Task<Result> ResendVerificationEmailAsync(string email);
    }
}
