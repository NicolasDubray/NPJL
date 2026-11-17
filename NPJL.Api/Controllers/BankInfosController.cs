
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Services;
using NPJL.Infrastructure.Identity;
using System.Security.Claims;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankInfosController : ControllerBase
    {
        private readonly BankInfoService _bankInfoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BankInfosController(
            BankInfoService bankInfoService,
            UserManager<ApplicationUser> userManager)
        {
            _bankInfoService = bankInfoService;
            _userManager = userManager;
        }

        // Retrieves banking details for a specific employee.
        // Administrators can access any employee.
        // Employees can only access their own banking details.
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null)
                {
                    return Forbid();
                }

                if (currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            var bankInfos =
                await _bankInfoService.GetByEmployeeIdAsync(employeeId);

            return Ok(bankInfos);
        }

        // Creates banking details for an employee.
        // Administrators can create information for any employee.
        // Employees can only create their own banking details.
        [HttpPost("employee/{employeeId:int}")]
        public async Task<IActionResult> Create(
            int employeeId,
            [FromBody] CreateBankInfoDto request)
        {
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null)
                {
                    return Forbid();
                }

                if (currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            var bankInfo = await _bankInfoService.CreateAsync(
                employeeId,
                request);

            return Ok(bankInfo);
        }

        // Updates existing banking details.
        // Administrators can update any employee.
        // Employees can only update their own banking details.
        [HttpPut("{bankInfoId:int}/employee/{employeeId:int}")]
        public async Task<IActionResult> Update(
            int bankInfoId,
            int employeeId,
            [FromBody] UpdateBankInfoDto request)
        {
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null)
                {
                    return Forbid();
                }

                if (currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            var updated = await _bankInfoService.UpdateAsync(
                employeeId,
                bankInfoId,
                request);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Retrieves EmployeeId for the currently authenticated user.
        private async Task<int?> GetCurrentEmployeeIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);

            return user?.EmployeeId;
        }
    }
}
