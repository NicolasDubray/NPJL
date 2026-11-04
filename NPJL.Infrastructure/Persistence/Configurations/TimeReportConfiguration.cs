using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class TimeReportConfiguration
        : IEntityTypeConfiguration<TimeReport>
    {
        public void Configure(EntityTypeBuilder<TimeReport> builder)
        {
            // Tabellnamn
            builder.ToTable("TimeReports", table =>
            {
                table.HasCheckConstraint(
                    "CK_TimeReports_TimeRange",
                    "[EndTime] > [StartTime]");

                table.HasCheckConstraint(
    "CK_TimeReports_BreakMinutes",
    "[BreakMinutes] >= 0 AND " +
    "[BreakMinutes] <= DATEDIFF(MINUTE, [StartTime], [EndTime])");
            });

            // Primärnyckel
            builder.HasKey(t => t.Id);

            // Arbetstid
            builder.Property(t => t.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(t => t.EndTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(t => t.BreakMinutes)
                .IsRequired();

            // Beskrivning
            builder.Property(t => t.Description)
                .HasMaxLength(1000)
                .IsRequired();

            // Skapandedatum
            builder.Property(t => t.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            // Relation till Employee
            builder.HasOne(t => t.Employee)
                .WithMany(e => e.TimeReports)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relation till Booking
            builder.HasOne(t => t.Booking)
                .WithMany(b => b.TimeReports)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för snabbare sökning
            builder.HasIndex(t => t.EmployeeId);
            builder.HasIndex(t => t.BookingId);
        }
    }
}
