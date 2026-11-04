using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class InvoiceBookingConfiguration
        : IEntityTypeConfiguration<InvoiceBooking>
    {
        public void Configure(EntityTypeBuilder<InvoiceBooking> builder)
        {
            builder.ToTable("Invoice_Bookings");

            // Sammansatt primärnyckel
            builder.HasKey(ib => new { ib.InvoiceId, ib.BookingId });

            // Relation till Invoice
            builder.HasOne(ib => ib.Invoice)
                .WithMany(i => i.InvoiceBookings)
                .HasForeignKey(ib => ib.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // En bokning får endast tillhöra en faktura
            builder.HasOne(ib => ib.Booking)
                .WithOne(b => b.InvoiceBooking)
                .HasForeignKey<InvoiceBooking>(ib => ib.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
