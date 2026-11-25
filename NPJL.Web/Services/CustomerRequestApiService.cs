using NPJL.Domain.Entities;
using System.Net.Http.Json;
using NPJL.Application.DTOs.Requests;
using NPJL.Domain.Enums;

namespace NPJL.Web.Services;

// Service för kommunikation med API:t
// gällande kundförfrågningar.
public class CustomerRequestApiService
{
    private readonly AuthService _authService;

    public CustomerRequestApiService(AuthService authService)
    {
        _authService = authService;
    }

    // Hämtar samtliga kundförfrågningar.
    // Kräver administratörsbehörighet.
    public async Task<IReadOnlyList<CustomerRequest>> GetAllAsync()
    {
        var client = _authService.CreateAuthenticatedClient();

        var requests = await client.GetFromJsonAsync<List<CustomerRequest>>(
            "api/CustomerRequests");

        return requests ?? new List<CustomerRequest>();
    }
    // Ändrar status på en kundförfrågan.
    // Kräver administratörsbehörighet.
    public async Task UpdateStatusAsync(
        int requestId,
        CustomerRequestStatus status)
    {
        if (requestId <= 0)
        {
            throw new ArgumentException("Invalid request ID.");
        }

        var client = _authService.CreateAuthenticatedClient();

        var response = await client.PutAsJsonAsync(
            $"api/CustomerRequests/{requestId}/status",
            new UpdateRequestStatusDto
            {
                Status = status
            });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                string.IsNullOrWhiteSpace(error)
                    ? $"Could not update request status. Status code: {response.StatusCode}"
                    : error,
                null,
                response.StatusCode);
        }
    }
    public async Task CreateAsync(CreateRequestDto dto)
    {
        var client = _authService.CreateAuthenticatedClient();

        var response = await client.PostAsJsonAsync(
            "api/CustomerRequests",
            dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                string.IsNullOrWhiteSpace(error)
                    ? $"Could not create customer request. Status code: {response.StatusCode}"
                    : error,
                null,
                response.StatusCode);
        }
    }
}