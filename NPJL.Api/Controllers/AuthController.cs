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

        public AuthController(
            ICustomerRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

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
    }
}