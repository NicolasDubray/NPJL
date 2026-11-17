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
    public class ContactInfosController : ControllerBase
    {
        private readonly ContactInfoService _contactInfoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContactInfosController(
            ContactInfoService contactInfoService,
            UserManager<ApplicationUser> userManager)
        {
            _contactInfoService = contactInfoService;
            _userManager = userManager;
        }

        // Retrieves contact information for a specific employee.
        // Administrators can access any employee.
        // Employees can only access their own contact information.
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

            var contactInfos =
                await _contactInfoService.GetByEmployeeIdAsync(employeeId);

            return Ok(contactInfos);
        }

        // Creates contact information for an employee.
        // Administrators can create information for any employee.
        // Employees can only create their own contact information.
        [HttpPost("employee/{employeeId:int}")]
        public async Task<IActionResult> Create(
            int employeeId,
            [FromBody] CreateContactInfoDto request)
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

            var contactInfo = await _contactInfoService.CreateAsync(
                employeeId,
                request);

            return Ok(new ContactInfoDto
            {
                Id = contactInfo.Id,
                Address = contactInfo.Address,
                Telephone = contactInfo.Telephone,
                Email = contactInfo.Email
            });
        }

        // Updates existing contact information.
        // Administrators can update any employee.
        // Employees can only update their own contact information.
        [HttpPut("{contactInfoId:int}/employee/{employeeId:int}")]
        public async Task<IActionResult> Update(
            int contactInfoId,
            int employeeId,
            [FromBody] UpdateContactInfoDto request)
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

            var updated = await _contactInfoService.UpdateAsync(
                employeeId,
                contactInfoId,
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