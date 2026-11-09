using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NPJL.Application.Services;

namespace NPJL.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<EmployeeService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<BookingService>();

            return services;
        }
    }
}