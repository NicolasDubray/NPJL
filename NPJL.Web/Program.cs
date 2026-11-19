using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using NPJL.Web.Configuration;
using NPJL.Infrastructure;
using NPJL.Web.Components;
using NPJL.Web.Services;

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
builder.Services.AddScoped<EmployeeApiService>();
builder.Services.AddScoped<ContactInfoApiService>();
builder.Services.AddScoped<AbsentismApiService>();
builder.Services.AddScoped<BankInfoApiService>();
builder.Services.AddScoped<TimeReportApiService>();
builder.Services.AddScoped<SalaryApiService>();

// Registrerar gemensamma skatteinställningar.
builder.Services.Configure<TaxSettings>(
    builder.Configuration.GetSection("TaxSettings"));
builder.Services.AddScoped<CurrentCustomerService>();

// Registrerar autentiseringshantering
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// Registrerar skyddad sessionslagring för autentisering.
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddInfrastructure(builder.Configuration);
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
