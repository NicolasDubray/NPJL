using NPJL.Application.DTOs.Employees;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service for communication with contact information API endpoints.
    public class ContactInfoApiService
    {
        private readonly AuthService _authService;

        public ContactInfoApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Retrieves contact information for a specific employee.
        public async Task<IReadOnlyList<ContactInfoDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            var client = _authService.CreateAuthenticatedClient();

            var contactInfos =
                await client.GetFromJsonAsync<List<ContactInfoDto>>(
                    $"api/ContactInfos/employee/{employeeId}");

            return contactInfos ?? new List<ContactInfoDto>();
        }

        // Creates contact information for an employee.
        public async Task<ContactInfoDto?> CreateAsync(
            int employeeId,
            CreateContactInfoDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                $"api/ContactInfos/employee/{employeeId}",
                request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ContactInfoDto>();
        }

        // Updates existing contact information.
        public async Task<bool> UpdateAsync(
            int employeeId,
            int contactInfoId,
            UpdateContactInfoDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/ContactInfos/{contactInfoId}/employee/{employeeId}",
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