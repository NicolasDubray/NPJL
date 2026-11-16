
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

    }
}
