
using NPJL.Application.DTOs.Absentisms;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service for communication with absence-related API endpoints.
    public class AbsentismApiService
    {
        private readonly AuthService _authService;

        public AbsentismApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Retrieves all absence entries for a specific employee.
        public async Task<IReadOnlyList<AbsentismDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            var client = _authService.CreateAuthenticatedClient();

            var absences = await client.GetFromJsonAsync<List<AbsentismDto>>(
                $"api/Absentisms/employee/{employeeId}");

            return absences ?? new List<AbsentismDto>();
        }

        // Registers an absence for the currently logged-in employee.
        public async Task<AbsentismDto?> CreateAsync(
            CreateAbsentismDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                "api/Absentisms",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<AbsentismDto>();
        }

        // Registers an absence for a specific employee.
        // Requires Administrator authorization.
        public async Task<AbsentismDto?> CreateForEmployeeAsync(
            int employeeId,
            CreateAbsentismDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                $"api/Absentisms/employee/{employeeId}",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<AbsentismDto>();
        }

        // Updates an existing absence entry.
        // Authorization and editing restrictions are handled by the API.
        public async Task<AbsentismDto?> UpdateAsync(
            int id,
            UpdateAbsentismDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/Absentisms/{id}",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<AbsentismDto>();
        }

        // Deletes an existing absence entry.
        // Requires Administrator authorization.
        public async Task DeleteAsync(int id)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.DeleteAsync(
                $"api/Absentisms/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }
        }
    }
}
