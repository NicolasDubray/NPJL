
using NPJL.Application.DTOs.Employees;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service for communication with salary API endpoints.
    public class SalaryApiService
    {
        private readonly AuthService _authService;

        public SalaryApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Retrieves salary history for a specific employee.
        public async Task<IReadOnlyList<SalaryDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            }

            var client = _authService.CreateAuthenticatedClient();

            var salaries =
                await client.GetFromJsonAsync<List<SalaryDto>>(
                    $"api/Salary/employee/{employeeId}");

            return salaries ?? new List<SalaryDto>();
        }

        // Retrieves the current salary for a specific employee.
        public async Task<SalaryDto?> GetCurrentByEmployeeIdAsync(
            int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            }

            var client = _authService.CreateAuthenticatedClient();

            var response = await client.GetAsync(
                $"api/Salary/employee/{employeeId}/current");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SalaryDto>();
        }

        // Registers a new salary period for an employee.
        public async Task<SalaryDto?> CreateAsync(
            int employeeId,
            CreateSalaryDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            }

            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                $"api/Salary/employee/{employeeId}",
                request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SalaryDto>();
        }
    }
}
