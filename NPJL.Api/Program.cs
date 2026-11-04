using Microsoft.EntityFrameworkCore;
using NPJL.Infrastructure.Identity;
using System;
using NPJL.Infrastructure.Persistence;
using NPJL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Registrerar API:ets controllers
builder.Services.AddControllers();

// Registrerar Infrastructure och ApplicationDbContext
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Kör migrationer automatiskt i utvecklingsmiljö
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
