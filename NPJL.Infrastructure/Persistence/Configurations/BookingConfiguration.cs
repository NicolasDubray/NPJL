using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class BookingConfiguration
        : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            // Tabellnamn och kontroll av bokningens tider
            builder.ToTable("Bookings", table =>
            {
                table.HasCheckConstraint(
                    "CK_Bookings_EndTime",
                    "[EndTime] > [StartTime]");
            });

            // Primärnyckel
            builder.HasKey(b => b.Id);

            // Bokningstider
            builder.Property(b => b.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(b => b.EndTime)
                .HasColumnType("datetime2")
                .IsRequired();

            // Bokningens status
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // Relation till Customer
            builder.HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för kundens bokningar
            builder.HasIndex(b => b.CustomerId);
        }
    }
}
