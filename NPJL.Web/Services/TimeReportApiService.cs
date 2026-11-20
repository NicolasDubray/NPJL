
using NPJL.Application.DTOs.Employees;
using System.Net.Http.Json;

namespace NPJL.Web.Services
{
    // Service for communication with time report-related API endpoints.
    public class TimeReportApiService
    {
        private readonly AuthService _authService;

        public TimeReportApiService(AuthService authService)
        {
            _authService = authService;
        }

        // Retrieves all time reports for a specific employee.
        public async Task<IReadOnlyList<TimeReportDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            var client = _authService.CreateAuthenticatedClient();

            var timeReports = await client.GetFromJsonAsync<List<TimeReportDto>>(
                $"api/TimeReports/employee/{employeeId}");

            return timeReports ?? new List<TimeReportDto>();
        }

        // Retrieves all time reports for a specific customer.
        // Requires Administrator authorization.
        public async Task<IReadOnlyList<TimeReportDto>> GetByCustomerIdAsync(
            int customerId)
        {
            var client = _authService.CreateAuthenticatedClient();

            var timeReports = await client.GetFromJsonAsync<List<TimeReportDto>>(
                $"api/TimeReports/customer/{customerId}");

            return timeReports ?? new List<TimeReportDto>();
        }

        // Retrieves time reports for an employee within a date range.
        public async Task<IReadOnlyList<TimeReportDto>>
            GetByEmployeeIdAndDateRangeAsync(
                int employeeId,
                DateTime startDate,
                DateTime endDate)
        {
            var client = _authService.CreateAuthenticatedClient();

            var timeReports = await client.GetFromJsonAsync<List<TimeReportDto>>(
                $"api/TimeReports/employee/{employeeId}/daterange" +
                $"?startDate={Uri.EscapeDataString(startDate.ToString("O"))}" +
                $"&endDate={Uri.EscapeDataString(endDate.ToString("O"))}");

            return timeReports ?? new List<TimeReportDto>();
        }

        // Registers a time report for the currently logged-in employee.
        public async Task<TimeReportDto?> CreateAsync(
            CreateTimeReportDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                "api/TimeReports",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<TimeReportDto>();
        }

        // Registers a time report for a specific employee.
        // Requires Administrator authorization.
        public async Task<TimeReportDto?> CreateForEmployeeAsync(
            int employeeId,
            CreateTimeReportDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PostAsJsonAsync(
                $"api/TimeReports/employee/{employeeId}",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<TimeReportDto>();
        }

        // Updates an existing time report.
        // Authorization is handled by the API.
        public async Task<TimeReportDto?> UpdateAsync(
            int id,
            UpdateTimeReportDto request)
        {
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.PutAsJsonAsync(
                $"api/TimeReports/{id}",
                request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    errorMessage,
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<TimeReportDto>();
        }
        
        // Deletes an existing time report.
        // Requires Administrator authorization.
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid time report ID.", nameof(id));
            }
            var client = _authService.CreateAuthenticatedClient();

            var response = await client.DeleteAsync(
                $"api/TimeReports/{id}");

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
