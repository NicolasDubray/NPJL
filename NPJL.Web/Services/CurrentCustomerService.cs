using Microsoft.AspNetCore.Components.Authorization;
using NPJL.Application.DTOs.Customers;
using NPJL.Application.Services;
using System.Security.Claims;

namespace NPJL.Web.Services;

public class CurrentCustomerService(AuthenticationStateProvider authenticationStateProvider, CustomerService customerService)
{
    public async Task<CustomerDto?> GetCurrentCustomerAsync()
    {

        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        var email = state.User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return null;
        return await customerService.GetProfileByEmailAsync(email);
    }
}
