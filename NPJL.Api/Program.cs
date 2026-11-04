
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPJL.Infrastructure;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Registrerar API:ets controllers
builder.Services.AddControllers();

// Registrerar Infrastructure och ApplicationDbContext
builder.Services.AddInfrastructure(builder.Configuration);

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

// Kör migrationer automatiskt i utvecklingsmiljö
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}
// Skapar Identity-roller om de saknas
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    await IdentityRoleSeeder.SeedRolesAsync(roleManager);
}
app.UseHttpsRedirection();

// Autentisering måste köras före behörighetskontroll
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
