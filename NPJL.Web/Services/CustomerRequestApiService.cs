using NPJL.Domain.Entities;
using System.Net.Http.Json;

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
}