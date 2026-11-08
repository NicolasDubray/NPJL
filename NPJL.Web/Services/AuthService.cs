using System.Net.Http.Json;
using NPJL.Application.DTOs.Authentication;

namespace NPJL.Web.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> RegisterCustomerAsync(
            CustomerRegistrationDto request)
        {
            var client = _httpClientFactory.CreateClient("NPJL.Api");

            return await client.PostAsJsonAsync(
                "api/Auth/register",
                request);
        }
    }
}