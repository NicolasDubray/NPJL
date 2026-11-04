using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class EmploymentConfiguration
        : IEntityTypeConfiguration<Employment>
    {
        public void Configure(EntityTypeBuilder<Employment> builder)
        {
            builder.ToTable("Employments", table =>
            {
                table.HasCheckConstraint(
                    "CK_Employments_NumWorkingHours",
                    "[NumWorkingHours] >= 0");
            });

            builder.HasKey(e => e.Id);

            builder.Property(e => e.HireType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.HireDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(e => e.Role)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Department)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.NumWorkingHours)
                .HasPrecision(6, 2)
                .IsRequired();

            builder.HasOne(e => e.Employee)
                .WithMany(emp => emp.Employments)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.EmployeeId);
        }
    }
}
