using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class AbsentismConfiguration
        : IEntityTypeConfiguration<Absentism>
    {
        public void Configure(EntityTypeBuilder<Absentism> builder)
        {
            // Tabellnamn
            builder.ToTable("Absentisms", table =>
            {
                table.HasCheckConstraint(
                    "CK_Absentisms_TimeRange",
                    "[EndTime] > [StartTime]");
            });

            // Primärnyckel
            builder.HasKey(a => a.Id);

            // Frånvaroperiod
            builder.Property(a => a.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(a => a.EndTime)
                .HasColumnType("datetime2")
                .IsRequired();

            // Frånvarotyp lagras som text
            builder.Property(a => a.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // Relation till Employee
            builder.HasOne(a => a.Employee)
                .WithMany(e => e.Absentisms)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för anställd
            builder.HasIndex(a => a.EmployeeId);
        }
    }
}
