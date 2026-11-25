using Microsoft.AspNetCore.Components.Authorization;
using NPJL.Application.DTOs.Customers;
using NPJL.Application.Services;
using System.Security.Claims;
using System.Net.Http.Json;

namespace NPJL.Web.Services;

public class CurrentCustomerService(AuthService authService)
{
    public async Task<CustomerDto?> GetCurrentCustomerAsync()
    {
        var client = authService.CreateAuthenticatedClient();

        return await client.GetFromJsonAsync<CustomerDto>(
            "api/Customers/my");
    }
}