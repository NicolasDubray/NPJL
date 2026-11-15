
using NPJL.Application.DTOs.Employees;
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
    }
}
