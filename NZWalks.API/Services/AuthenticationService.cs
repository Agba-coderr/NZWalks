using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthenticationService(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                    return new LoginResponseDto
                    {
                        Token = jwtToken,
                        UserId = user.Id,
                        Roles = roles.ToList()
                    };
                }
            }

            return null;
        }

        // Return IdentityResult so controller can return specific errors to client
        public async Task<IdentityResult> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
                return identityResult;

            // Public registration: default to Reader role. Do not allow callers to assign roles.
            var defaultRoles = new[] { "Reader" };
            identityResult = await userManager.AddToRolesAsync(identityUser, defaultRoles);

            return identityResult;
        }
    }
}