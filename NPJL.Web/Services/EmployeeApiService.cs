
using NPJL.Application.DTOs.Employees;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service för kommunikation med personalrelaterade API-endpoints.
    public class EmployeeApiService
    {
        private readonly AuthService _authService;

        public EmployeeApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Hämtar samtliga anställda från API:t.
        // Kräver administratörsbehörighet.
        public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync()
        {
            var client = _authService.CreateAuthenticatedClient();

            var employees = await client.GetFromJsonAsync<List<EmployeeDto>>(
                "api/Employees");

            return employees ?? new List<EmployeeDto>();
        }
        
        // Hämtar detaljerad information om en anställd.
        // Administratörer får hämta valfri anställd.
        // Övriga användare får endast hämta sina egna uppgifter.
        public async Task<EmployeeDetailsDto?> GetDetailsAsync(int id)
        {
            var client = _authService.CreateAuthenticatedClient();

            return await client.GetFromJsonAsync<EmployeeDetailsDto>(
                $"api/Employees/{id}/details");
        }

        // Retrieves detailed information about the currently logged-in employee.
        public async Task<EmployeeDetailsDto?> GetMyDetailsAsync()
        {
            var client = _authService.CreateAuthenticatedClient();

            return await client.GetFromJsonAsync<EmployeeDetailsDto>(
                "api/Employees/me/details");
        }

        // Updates an employee's vacation days.
        // Requires administrator authorization.
        public async Task UpdateVacationDaysAsync(
            int employeeId,
            UpdateVacationDaysDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }
            
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/Employees/{employeeId}/vacation-days",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    string.IsNullOrWhiteSpace(error)
                        ? $"Could not update vacation days. Status code: {response.StatusCode}"
                        : error,
                    null,
                    response.StatusCode);
            }
        }

        // Updates an employee's active status.
        // Requires administrator authorization.
        public async Task UpdateEmployeeStatusAsync(
            int employeeId,
            UpdateEmployeeStatusDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/Employees/{employeeId}/status",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    string.IsNullOrWhiteSpace(error)
                        ? $"Could not update employee status. Status code: {response.StatusCode}"
                        : error,
                    null,
                    response.StatusCode);
            }
        }
        // Registers a new employee or administrator.
        // Requires administrator authorization.
        public async Task<CreateEmployeeResultDto> RegisterEmployeeAsync(
            CreateEmployeeDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                "api/EmployeeRegistration",
                request);

            var result = await response.Content.ReadFromJsonAsync<CreateEmployeeResultDto>();

            if (result != null)
            {
                return result;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    string.IsNullOrWhiteSpace(error)
                        ? $"Employee registration failed. Status code: {response.StatusCode}"
                        : error,
                    null,
                    response.StatusCode);
            }

            throw new HttpRequestException(
                "The API returned an unexpected response during registration.");
        }



    }
}
