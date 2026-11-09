using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace NPJL.Web.Services
{
    public class CustomAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        private ClaimsPrincipal _currentUser =
            new ClaimsPrincipal(new ClaimsIdentity());

        private bool _sessionInitialized = false;

        public string? Token { get; private set; }

        public CustomAuthenticationStateProvider(
            ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            // Återställer autentiseringstillståndet från webbläsarens session.
            if (!_sessionInitialized)
            {
                try
                {
                    var result = await _sessionStorage.GetAsync<SessionData>(
                        "Authentication");

                    if (result.Success && result.Value != null)
                    {
                        var session = result.Value;

                        Token = session.Token;

                        SetCurrentUser(
                            session.Email,
                            session.Roles);
                    }

                    _sessionInitialized = true;
                }
                catch (InvalidOperationException)
                {
                    // JavaScript kan inte användas under serverns prerendering.
                    // Försöket görs igen när sidan blivit interaktiv.
                }
            }

            return new AuthenticationState(_currentUser);
        }

        public void Login(
            string token,
            string email,
            IEnumerable<string> roles)
        {
            Token = token;

            SetCurrentUser(email, roles);

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        // Sparar inloggningsinformationen i den aktuella webbläsarsessionen.
        public async Task LoginAsync(
            string token,
            string email,
            IEnumerable<string> roles)
        {
            var roleList = roles.ToList();

            Login(token, email, roleList);

            await _sessionStorage.SetAsync(
                "Authentication",
                new SessionData
                {
                    Token = token,
                    Email = email,
                    Roles = roleList
                });
        }

        public void Logout()
        {
            Token = null;

            _currentUser = new ClaimsPrincipal(
                new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        // Tar bort den sparade inloggningen från webbläsarsessionen.
        public async Task LogoutAsync()
        {
            Logout();

            await _sessionStorage.DeleteAsync("Authentication");
        }

        // Skapar användarens ClaimsPrincipal utifrån e-postadress och roller.
        private void SetCurrentUser(
            string email,
            IEnumerable<string> roles)
        {
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
        }

        // Innehåller informationen som sparas under webbläsarsessionen.
        private class SessionData
        {
            public string Token { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new();
        }
    }
}