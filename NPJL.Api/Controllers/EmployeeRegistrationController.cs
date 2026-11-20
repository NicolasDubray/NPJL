using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class EmployeeRegistrationController : ControllerBase
    {
        private readonly IEmployeeRegistrationService _employeeRegistrationService;

        public EmployeeRegistrationController(
            IEmployeeRegistrationService employeeRegistrationService)
        {
            _employeeRegistrationService = employeeRegistrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] CreateEmployeeDto request)
        {
            try
            {
                var result = await _employeeRegistrationService.RegisterAsync(
                    request);

                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An unexpected error occurred during registration."
                    });
            }
        }
    }
}