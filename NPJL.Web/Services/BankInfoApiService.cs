
using NPJL.Application.DTOs.Employees;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service for communication with banking information API endpoints.
    public class BankInfoApiService
    {
        private readonly AuthService _authService;

        public BankInfoApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Retrieves banking details for a specific employee.
        public async Task<IReadOnlyList<BankInfoDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            var client = _authService.CreateAuthenticatedClient();

            var bankInfos =
                await client.GetFromJsonAsync<List<BankInfoDto>>(
                    $"api/BankInfos/employee/{employeeId}");

            return bankInfos ?? new List<BankInfoDto>();
        }

        // Creates banking details for an employee.
        public async Task<BankInfoDto?> CreateAsync(
            int employeeId,
            CreateBankInfoDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                $"api/BankInfos/employee/{employeeId}",
                request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<BankInfoDto>();
        }

        // Updates existing banking details.
        public async Task<bool> UpdateAsync(
            int employeeId,
            int bankInfoId,
            UpdateBankInfoDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/BankInfos/{bankInfoId}/employee/{employeeId}",
                request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
