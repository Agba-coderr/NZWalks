using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IEmailService _emailService;

        public AuthenticationService(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                    await _emailService.SendEmailAsync(
                        toEmail: user.Email!,
                        subject: "New Login to Your NZ Walks Account",
                        body: $@"
                            <p>Hi <b>{user.UserName}</b>,</p>
                            <p>We noticed a new login to your NZ Walks account.</p>
                            <p>If this was you, no action is required.</p>
                            <p>If you did not make this login, please secure your account by changing your password and reviewing your account activity.</p>
                            <br/>
                            <p>Best regards,<br/><b>The NZ Walks Team</b></p>"
                    );

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

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
                return identityResult;

            await _emailService.SendEmailAsync(
                toEmail: identityUser.Email,
                subject: "Welcome to NZ Walks!",
                body: $@"
                    <p>Hi <b>{identityUser.UserName}</b>,</p>
                    <p>Welcome to NZ Walks!</p>
                    <p>Your account has been successfully created, and you’re all set to start discovering some of New Zealand’s beautiful walks and trails.</p>
                    <p>Happy exploring!</p>
                    <br/>
                    <p>Best regards,<br/><b>The NZ Walks Team</b></p>"
            );

            // Public registration: default to Reader role. Do not allow callers to assign roles.
            var defaultRoles = new[] { "Reader" };
            identityResult = await _userManager.AddToRolesAsync(identityUser, defaultRoles);

            return identityResult;
        }
    }
}