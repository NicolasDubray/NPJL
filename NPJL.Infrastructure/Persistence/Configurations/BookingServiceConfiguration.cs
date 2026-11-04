using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class BookingServiceConfiguration
        : IEntityTypeConfiguration<BookingService>
    {
        public void Configure(EntityTypeBuilder<BookingService> builder)
        {
            builder.ToTable("Bookings_Service", table =>
            {
                table.HasCheckConstraint(
                    "CK_Bookings_Service_PriceAtBooking",
                    "[PriceAtBooking] >= 0");
            });

            builder.HasKey(bs => new { bs.BookingId, bs.ServiceId });

            // Historiskt pris med två decimaler
            builder.Property(bs => bs.PriceAtBooking)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bs => bs.Service)
                .WithMany()
                .HasForeignKey(bs => bs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
