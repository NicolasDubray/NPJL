using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Authentication;
using NPJL.Application.Interfaces;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRegistrationService _registrationService;
        private readonly IUserAuthenticationService _authenticationService;

        public AuthController(
            ICustomerRegistrationService registrationService,
            IUserAuthenticationService authenticationService)
        {
            _registrationService = registrationService;
            _authenticationService = authenticationService;
        }

        // Kundregistrering
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] CustomerRegistrationDto request)
        {
            var result = await _registrationService.RegisterAsync(request);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // Inloggning
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto request)
        {
            var result = await _authenticationService.LoginAsync(request);

            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}