using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Domain.Entities
{
    public class BookingTeam
    {
        // Sammansatt primärnyckel
        public int BookingId { get; set; }
        public int TeamId { get; set; }

        // Navigationsegenskaper
        public Booking Booking { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }
}