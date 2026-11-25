
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPJL.Api.Development;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;
using NPJL.Infrastructure.Services;
using System.Text;
using NPJL.Application;

var builder = WebApplication.CreateBuilder(args);

// Registrerar API:ets controllers
builder.Services.AddControllers();

// Registrerar Infrastructure och ApplicationDbContext
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Hämtar JWT-inställningar
var jwtSettings = builder.Configuration.GetSection("Jwt");

var secretKey = jwtSettings["Secret"]
    ?? throw new InvalidOperationException("JWT Secret is missing.");

var key = Encoding.UTF8.GetBytes(secretKey);

// Registrerar JWT-autentisering
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(key),

            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

var app = builder.Build();

// Kör migrationer och utvecklingsseedning.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    // Kör eventuella väntande databasmigrationer.
    await dbContext.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();
    
    // Roller måste finnas innan användaren tilldelas en roll.
    await IdentityRoleSeeder.SeedRolesAsync(roleManager);

    // Skapar testanställd och tillhörande inloggningskonto.
    await TestEmployeeSeeder.SeedAsync(dbContext, userManager);

    // Skapar testadministratör och tillhörande inloggningskonto.
    await AdministratorSeeder.SeedAsync(userManager);
    // Skapar testdata för kundens bokningar och fakturor.
    await DevelopmentDataSeeder.SeedAsync(dbContext, "Test@test.com");
}

app.UseHttpsRedirection();

// Autentisering måste köras före behörighetskontroll
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
