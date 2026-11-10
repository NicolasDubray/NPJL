using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    // Konfiguration för anställdas löneuppgifter.
    public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.ToTable("Salaries", table =>
            {
                // Lönebeloppet måste vara större än noll.
                table.HasCheckConstraint(
                    "CK_Salaries_Amount",
                    "[Amount] > 0");

                // Slutdatum får inte vara tidigare än startdatum.
                table.HasCheckConstraint(
                    "CK_Salaries_ValidTo",
                    "[ValidTo] IS NULL OR [ValidTo] >= [ValidFrom]");
            });

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(s => s.ValidFrom)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(s => s.ValidTo)
                .HasColumnType("date");

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            // Koppling till anställd.
            // DeleteBehavior.Restrict förhindrar att lönehistorik
            // raderas automatiskt när en anställd tas bort.
            builder.HasOne(s => s.Employee)
                .WithMany(e => e.Salaries)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för effektiv hämtning av löneuppgifter per anställd.
            builder.HasIndex(s => s.EmployeeId);
        }
    }
}
