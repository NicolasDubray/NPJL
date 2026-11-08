using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace NPJL.Web.Services
{
    public class CustomAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser =
            new ClaimsPrincipal(new ClaimsIdentity());

        public string? Token { get; private set; }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(
                new AuthenticationState(_currentUser));
        }

        public void Login(
            string token,
            string email,
            IEnumerable<string> roles)
        {
            Token = token;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                authenticationType: "JWT");

            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        public void Logout()
        {
            Token = null;

            _currentUser = new ClaimsPrincipal(
                new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }
    }
}