
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Employees;
using NPJL.Infrastructure.Identity;
using NPJL.Application.Services;
using System.Security.Claims;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalaryController : ControllerBase
    {
        private readonly SalaryService _salaryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalaryController(
            SalaryService salaryService,
            UserManager<ApplicationUser> userManager)
        {
            _salaryService = salaryService;
            _userManager = userManager;
        }

        // Hämtar lönehistoriken för en anställd.
        // Administratörer får hämta valfri anställd.
        // Övriga användare får endast hämta sina egna löneuppgifter.
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }

            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null ||
                    currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            var salaries = await _salaryService.GetByEmployeeIdAsync(
                employeeId);

            var salaryDtos = salaries.Select(s => new SalaryDto
            {
                Id = s.Id,
                Amount = s.Amount,
                ValidFrom = s.ValidFrom,
                ValidTo = s.ValidTo,
                CreatedAt = s.CreatedAt
            }).ToList();

            return Ok(salaryDtos);
        }

        // Hämtar den aktuella lönen för en anställd.
        // Administratörer får hämta valfri anställd.
        // Övriga användare får endast hämta sina egna löneuppgifter.
        [HttpGet("employee/{employeeId:int}/current")]
        public async Task<IActionResult> GetCurrentByEmployeeId(
            int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }

            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null ||
                    currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            var salary = await _salaryService.GetCurrentByEmployeeIdAsync(
                employeeId);

            if (salary == null)
            {
                return NotFound("No current salary found.");
            }

            var salaryDto = new SalaryDto
            {
                Id = salary.Id,
                Amount = salary.Amount,
                ValidFrom = salary.ValidFrom,
                ValidTo = salary.ValidTo,
                CreatedAt = salary.CreatedAt
            };

            return Ok(salaryDto);
        }

        // Registrerar en ny löneperiod för en anställd.
        // Endast administratörer får använda denna metod.
        [HttpPost("employee/{employeeId:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RegisterSalaryChange(
            int employeeId,
            [FromBody] CreateSalaryDto request)
        {
            if (employeeId <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }

            try
            {
                var salary = await _salaryService.RegisterSalaryChangeAsync(
                    employeeId,
                    request.Amount,
                    request.ValidFrom,
                    request.ValidTo);

                var salaryDto = new SalaryDto
                {
                    Id = salary.Id,
                    Amount = salary.Amount,
                    ValidFrom = salary.ValidFrom,
                    ValidTo = salary.ValidTo,
                    CreatedAt = salary.CreatedAt
                };

                return Ok(salaryDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Hämtar EmployeeId för den inloggade användaren.
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
