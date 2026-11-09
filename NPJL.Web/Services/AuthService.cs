using System.Net.Http.Json;
using NPJL.Application.DTOs.Authentication;
using System.Net.Http.Headers;

namespace NPJL.Web.Services
{
    
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CustomAuthenticationStateProvider _authenticationProvider;
        public AuthService(
    IHttpClientFactory httpClientFactory,
    CustomAuthenticationStateProvider authenticationProvider)
        {
            _httpClientFactory = httpClientFactory;
            _authenticationProvider = authenticationProvider;
        }

        public async Task<HttpResponseMessage> RegisterCustomerAsync(
            CustomerRegistrationDto request)
        {
            var client = _httpClientFactory.CreateClient("NPJL.Api");

            return await client.PostAsJsonAsync(
                "api/Auth/register",
                request);
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginDto request)
        {
            var client = _httpClientFactory.CreateClient("NPJL.Api");

            return await client.PostAsJsonAsync(
                "api/Auth/login",
                request);
        }
        // Skapar en HttpClient som skickar med användarens JWT-token.
        public HttpClient CreateAuthenticatedClient()
        {
            var client = _httpClientFactory.CreateClient("NPJL.Api");

            if (!string.IsNullOrWhiteSpace(_authenticationProvider.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        _authenticationProvider.Token);
            }

            return client;
        }
        
    }
}
