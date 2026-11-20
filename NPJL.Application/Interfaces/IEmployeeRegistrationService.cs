using NPJL.Application.DTOs.Employees;

namespace NPJL.Application.Interfaces
{
    // Interface for registering new employees and administrator accounts.
    public interface IEmployeeRegistrationService
    {
        // Registers an employee and creates the associated Identity account.
        Task<CreateEmployeeResultDto> RegisterAsync(
            CreateEmployeeDto request);
    }
}