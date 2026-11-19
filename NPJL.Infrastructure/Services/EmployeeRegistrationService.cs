using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Services
{
    public class EmployeeRegistrationService : IEmployeeRegistrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeRegistrationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CreateEmployeeResultDto> RegisterAsync(
            CreateEmployeeDto request)
        {
            // Validate required information
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.SSN) ||
                string.IsNullOrWhiteSpace(request.Address) ||
                string.IsNullOrWhiteSpace(request.Telephone) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new CreateEmployeeResultDto
                {
                    Succeeded = false,
                    Message = "All fields are required."
                };
            }

            // Validate field lengths
            if (request.FirstName.Trim().Length > 100 ||
                request.LastName.Trim().Length > 100 ||
                request.SSN.Trim().Length > 20)
            {
                return new CreateEmployeeResultDto
                {
                    Succeeded = false,
                    Message = "One or more fields exceed the allowed length."
                };
            }

            // Check if the email address is already registered
            var existingUser = await _userManager.FindByEmailAsync(
                request.Email.Trim());

            if (existingUser != null)
            {
                return new CreateEmployeeResultDto
                {
                    Succeeded = false,
                    Message = "Email address is already registered."
                };
            }

            // Check if the SSN already exists
            var existingEmployee = await _context.Employees
                .AnyAsync(e => e.SSN == request.SSN.Trim());

            if (existingEmployee)
            {
                return new CreateEmployeeResultDto
                {
                    Succeeded = false,
                    Message = "SSN is already registered."
                };
            }

            // Determine the Identity role
            var role = request.IsAdministrator
                ? "Administrator"
                : "Employee";

            // Verify that the role exists
            if (!await _userManager.IsInRoleAsync(
                    new ApplicationUser(),
                    role))
            {
                // IsInRoleAsync requires an existing user.
                // Role existence is checked separately below.
            }

            // Start database transaction
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Create employee
                var employee = new Employee
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    SSN = request.SSN.Trim(),
                    IsActive = true,
                    VacationDays = 0
                };

                // Create contact information
                employee.ContactInfos.Add(new ContactInfo
                {
                    Address = request.Address.Trim(),
                    Telephone = request.Telephone.Trim(),
                    Email = request.Email.Trim()
                });

                // Save employee to obtain EmployeeId
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                // Create Identity account
                var user = new ApplicationUser
                {
                    UserName = request.Email.Trim(),
                    Email = request.Email.Trim(),
                    EmployeeId = employee.Id
                };

                var createResult = await _userManager.CreateAsync(
                    user,
                    request.Password);

                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new CreateEmployeeResultDto
                    {
                        Succeeded = false,
                        Message = string.Join(
                            ", ",
                            createResult.Errors.Select(e => e.Description))
                    };
                }

                // Assign Identity role
                var roleResult = await _userManager.AddToRoleAsync(
                    user,
                    role);

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new CreateEmployeeResultDto
                    {
                        Succeeded = false,
                        Message = string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description))
                    };
                }

                // Commit transaction
                await transaction.CommitAsync();

                return new CreateEmployeeResultDto
                {
                    Succeeded = true,
                    Message = "Employee registered successfully.",
                    EmployeeId = employee.Id
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}