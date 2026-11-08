using NPJL.Web.Components;
using NPJL.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Registrerar HttpClient för kommunikation med NPJL.Api
builder.Services.AddHttpClient("NPJL.Api", client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"];
    
    client.BaseAddress = new Uri(apiUrl
        ?? throw new InvalidOperationException(
            "API BaseUrl is missing from configuration."));
});
builder.Services.AddScoped<AuthService>();

// Registrerar autentiseringshantering
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddCascadingAuthenticationState();
// Registrerar skyddad sessionslagring för autentisering.
builder.Services.AddScoped<ProtectedSessionStorage>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
