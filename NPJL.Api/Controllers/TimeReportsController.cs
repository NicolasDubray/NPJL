
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure.Identity;
using System.Security.Claims;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TimeReportsController : ControllerBase
    {
        

        private readonly ITimeReportService _timeReportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimeReportsController(
            ITimeReportService timeReportService,
            UserManager<ApplicationUser> userManager)
        {
            _timeReportService = timeReportService;
            _userManager = userManager;
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

        // Hämtar tidrapporter för en anställd.
        // Administratörer får hämta valfri anställds tidrapporter.
        // Anställda får endast hämta sina egna.
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
            
            var timeReports = await _timeReportService
                .GetByEmployeeIdAsync(employeeId);

            return Ok(timeReports);
        }

        // Hämtar tidrapporter för en kund.
        // Endast administratörer får utföra denna sökning.
        [HttpGet("customer/{customerId:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }

            var timeReports = await _timeReportService
                .GetByCustomerIdAsync(customerId);

            return Ok(timeReports);
        }

        // Registrerar en tidrapport för den inloggade anställda.
        // Anställda får endast registrera tid för sig själva.
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTimeReportDto request)
        {
            // Administratörer använder den separata employee-endpointen.
            if (User.IsInRole("Administrator"))
            {
                return BadRequest(
                    "Administrator registration requires a separate employeeId.");
            }

            var currentEmployeeId = await GetCurrentEmployeeIdAsync();

            if (currentEmployeeId == null)
            {
                return Forbid();
            }

            try
            {
                var timeReport = await _timeReportService.CreateAsync(
                    currentEmployeeId.Value,
                    request);

                return Ok(timeReport);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Administratörer kan registrera tid åt valfri anställd.
        [HttpPost("employee/{employeeId:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateForEmployee(
            int employeeId,
            [FromBody] CreateTimeReportDto request)
        {
            try
            {
                var timeReport = await _timeReportService.CreateAsync(
                    employeeId,
                    request);

                return Ok(timeReport);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        // Anställda får endast söka på sig själva.
        [HttpGet("employee/{employeeId:int}/daterange")]
        public async Task<IActionResult> GetByDateRange(
            int employeeId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
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
            
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }

            var timeReports = await _timeReportService
                .GetByEmployeeIdAndDateRangeAsync(
                    employeeId,
                    startDate,
                    endDate);

            return Ok(timeReports);
        }

        // Uppdaterar en befintlig tidrapport.
        // Administratörer får uppdatera alla tidrapporter.
        // Anställda får endast uppdatera sina egna.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateTimeReportDto request)
        {
            var isAdministrator = User.IsInRole("Administrator");

            // Hämtar den befintliga tidrapporten.
            var existingTimeReport =
                await _timeReportService.GetByIdAsync(id);

            if (existingTimeReport == null)
            {
                return NotFound();
            }

            int employeeId;

            if (isAdministrator)
            {
                // Administratören använder rapportens befintliga ägare.
                // Byte av ägare tillåts inte genom denna endpoint.
                employeeId = existingTimeReport.EmployeeId;
            }
            else
            {
                var currentEmployeeId = await GetCurrentEmployeeIdAsync();

                if (currentEmployeeId == null ||
                    existingTimeReport.EmployeeId != currentEmployeeId.Value)
                {
                    return Forbid();
                }

                employeeId = currentEmployeeId.Value;
            }

            try
            {
                var updated = await _timeReportService.UpdateAsync(
                    id,
                    employeeId,
                    request);

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Deletes a time report.
        // Only administrators are allowed to perform this operation.
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid time report ID.");
            }

            try
            {
                await _timeReportService.DeleteAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Time report not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
