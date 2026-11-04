using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class InvoiceBooking
    {
        // Sammansatt primärnyckel
        public int InvoiceId { get; set; }
        public int BookingId { get; set; }

        // Navigationsegenskaper
        public Invoice Invoice { get; set; } = null!;
        public Booking Booking { get; set; } = null!;
    }
}
