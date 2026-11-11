
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NPJL.Infrastructure.Identity;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public EmployeesController(
    EmployeeService employeeService,
    UserManager<ApplicationUser> userManager)
        {
            _employeeService = employeeService;
            _userManager = userManager;
        }

        // Hämtar samtliga anställda.
        // Endast administratörer får använda denna metod.
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllDetailsAsync();

            return Ok(employees);
        }

        // Hämtar en anställd via ID.
        // Administratörer får hämta valfri anställd.
        // Övriga användare får endast hämta sina egna uppgifter.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            // Administratörer får tillgång till alla anställda.
            if (!User.IsInRole("Administrator"))
            {
                // Hämtar den inloggade användarens EmployeeId.
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                // Användaren saknar koppling till en anställd.
                if (currentEmployeeId == null)
                {
                    return Forbid();
                }

                // Kontrollerar att användaren endast hämtar sina egna uppgifter.
                if (currentEmployeeId.Value != id)
                {
                    return Forbid();
                }
            }

            var employee = await _employeeService.GetDetailsByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
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
        // Hämtar samtliga aktiva anställda.
        // Endast administratörer får använda denna metod.
        [HttpGet("active")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllActive()
        {
            var employees = await _employeeService.GetAllActiveDetailsAsync();

            return Ok(employees);
        }
    }
}
