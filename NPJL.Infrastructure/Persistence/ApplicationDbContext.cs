using Microsoft.EntityFrameworkCore;
using NPJL.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NPJL.Domain.Entities;


namespace NPJL.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Kör först Identity:s egna konfigurationer
            base.OnModelCreating(builder);

            // Laddar automatiskt alla IEntityTypeConfiguration
            // som finns i samma assembly som ApplicationDbContext
            builder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }

        // Huvudtabeller
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Employment> Employments => Set<Employment>();
        public DbSet<Salary> Salaries => Set<Salary>();
        public DbSet<PaymentDetails> PaymentDetails => Set<PaymentDetails>();
        public DbSet<BankInfo> BankInfos => Set<BankInfo>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Absentism> Absentisms => Set<Absentism>();
        public DbSet<TimeReport> TimeReports => Set<TimeReport>();

        // Kopplingstabeller
        public DbSet<BookingService> BookingServices => Set<BookingService>();
        public DbSet<BookingTeam> BookingTeams => Set<BookingTeam>();
        public DbSet<TeamEmployee> TeamEmployees => Set<TeamEmployee>();
        public DbSet<InvoiceBooking> InvoiceBookings => Set<InvoiceBooking>();
    }
}
