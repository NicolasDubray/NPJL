using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Customers;
using NPJL.Application.Services;
using System.Security.Claims;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CustomersController(CustomerService customerService) : ControllerBase
    {
        // Hämtar profilen för den inloggade kunden.
        [HttpGet("my")]
        public async Task<ActionResult<CustomerDto>> GetMyProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            try
            {
                var customer = await customerService.GetProfileByEmailAsync(email);

                return Ok(customer);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}