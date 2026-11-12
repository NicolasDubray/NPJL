
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.Services;
using NPJL.Infrastructure.Identity;
using System.Security.Claims;
using NPJL.Application.DTOs.Absentisms;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AbsentismsController : ControllerBase
    {
        private readonly AbsentismService _absentismService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AbsentismsController(
            AbsentismService absentismService,
            UserManager<ApplicationUser> userManager)
        {
            _absentismService = absentismService;
            _userManager = userManager;
        }

        // Hämtar frånvaroposter för en anställd.
        // Administratörer får hämta valfri anställds frånvaro.
        // Övriga användare får endast hämta sin egen frånvaro.
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();
                
                if (currentEmployeeId == null ||
                    currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }
            
            var absences = await _absentismService.GetByEmployeeIdDetailsAsync(employeeId);

            return Ok(absences);
        }

        // Hämtar frånvaroposter inom ett datumintervall.
        // Administratörer får söka på valfri anställd.
        // Övriga användare får endast söka på sig själva.
        [HttpGet("employee/{employeeId:int}/daterange")]
        public async Task<IActionResult> GetByDateRange(
            int employeeId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null ||
                    currentEmployeeId.Value != employeeId)
                {
                    return Forbid();
                }
            }

            if (startDate > endDate)
            {
                return BadRequest("Startdatum får inte vara senare än slutdatum.");
            }

            var absences = await _absentismService.GetByDateRangeDetailsAsync(
    employeeId,
    startDate,
    endDate);

            return Ok(absences);
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

        // Registrerar frånvaro för den inloggade anställda.
        // Administratörer kan också registrera frånvaro åt andra anställda.
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAbsentismDto request)
        {
            int employeeId;

            if (User.IsInRole("Administrator"))
            {
                // Administratören måste ange vilken anställd frånvaron gäller.
                return BadRequest(
                    "Administratörsregistrering kräver ett separat employeeId.");
            }

            var currentEmployeeId = await GetCurrentEmployeeIdAsync();

            if (currentEmployeeId == null)
            {
                return Forbid();
            }

            employeeId = currentEmployeeId.Value;

            try
            {
                var absentism = await _absentismService.CreateAsync(
                    employeeId,
                    request);

                return CreatedAtAction(
                    nameof(GetByEmployeeId),
                    new { employeeId },
                    absentism);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Administratörer kan registrera frånvaro åt valfri anställd.
        [HttpPost("employee/{employeeId:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateForEmployee(
            int employeeId,
            [FromBody] CreateAbsentismDto request)
        {
            try
            {
                var absentism = await _absentismService.CreateAsync(
                    employeeId,
                    request);

                return CreatedAtAction(
                    nameof(GetByEmployeeId),
                    new { employeeId },
                    absentism);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Uppdaterar en frånvaropost.
        // Administratörer får uppdatera valfri frånvaropost.
        // Anställda får endast uppdatera sin egen frånvaro.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAbsentismDto request)
        {
            // Kontrollerar behörighet för vanliga anställda.
            if (!User.IsInRole("Administrator"))
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null)
                {
                    return Forbid();
                }

                // Hämtar frånvaroposten för att kontrollera ägarskap.
                var existingAbsence = await _absentismService.GetByIdAsync(id);

                if (existingAbsence == null)
                {
                    return NotFound();
                }

                if (existingAbsence.EmployeeId != currentEmployeeId.Value)
                {
                    return Forbid();
                }
            }

            try
            {
                var updatedAbsence = await _absentismService.UpdateAsync(id, request);

                if (updatedAbsence == null)
                {
                    return NotFound();
                }

                return Ok(updatedAbsence);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Tar bort en frånvaropost.
        // Endast administratörer får utföra denna åtgärd.
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            // Kontrollerar att frånvaroposten finns.
            var absentism = await _absentismService.GetByIdAsync(id);

            if (absentism == null)
            {
                return NotFound();
            }

            await _absentismService.DeleteAsync(id);

            return NoContent();
        }




    }
}
