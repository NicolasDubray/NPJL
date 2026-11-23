using Microsoft.Extensions.DependencyInjection;
using NPJL.Application.Interfaces;
using NPJL.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
    this IServiceCollection services)
        {
            // Employee
            services.AddScoped<EmployeeService>();

            // Customer
            services.AddScoped<CustomerService>();

            // Booking
            services.AddScoped<BookingService>();

            // ContactInfo
            services.AddScoped<ContactInfoService>();

            // Absentism
            services.AddScoped<AbsentismService>();

            // BankInfo
            services.AddScoped<BankInfoService>();

            // TimeReport
            services.AddScoped<ITimeReportService, TimeReportService>();

            // Invoice
            services.AddScoped<InvoiceService>();

            // PaymentDetails
            services.AddScoped<PaymentDetailsService>();

            // CustomerRequest
            services.AddScoped<CustomerRequestService>();

            // Salary
            services.AddScoped<SalaryService>();

            return services;
        }
    }
}