using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Domain.Entities
{
    public class BookingService
    {
        // Sammansatt primärnyckel
        public int BookingId { get; set; }
        public int ServiceId { get; set; }

        // Priset på tjänsten när den kopplades till bokningen.
        public decimal PriceAtBooking { get; set; }

        // Navigationsegenskaper
        public Booking Booking { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}