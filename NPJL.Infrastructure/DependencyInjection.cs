
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure.Repositories;
using NPJL.Application.Services;
using NPJL.Infrastructure.Services;

namespace NPJL.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registrerar ApplicationDbContext med SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Registrerar ASP.NET Core Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                // Lösenordskrav
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Registrerar repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // Registrerar services
            services.AddScoped<NPJL.Application.Services.CustomerService>();
            services.AddScoped<SalaryService>();
            services.AddScoped<NPJL.Application.Services.EmployeeService>();

            // Registrerar kundregistreringstjänsten
            services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

            return services;
        }
    }
}
