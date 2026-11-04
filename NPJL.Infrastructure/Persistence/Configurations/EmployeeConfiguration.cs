using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;


namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration
        : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", table =>
            {
                table.HasCheckConstraint(
                    "CK_Employees_VacationDays",
                    "[VacationDays] >= 0 AND [VacationDays] <= 365");
            });

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.SSN)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.Property(e => e.VacationDays)
                .IsRequired();

            // Personnummer ska vara unikt
            builder.HasIndex(e => e.SSN)
                .IsUnique();
        }
    }
}
