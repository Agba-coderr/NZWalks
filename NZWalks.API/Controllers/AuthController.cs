using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var registerDto = await authenticationService.RegisterAsync(registerRequestDto);

            if (registerDto == null)
            {
                return BadRequest("Something went wrong");
            }

            return Ok("User was registered! Please login");
        }

        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task <IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authenticationService.LoginAsync(loginRequestDto);

            if (loginResponse == null)
            {
                return BadRequest("Username or password incorrect");
            }

            return Ok(loginResponse);
        }

    }
}
