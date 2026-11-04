using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class TimeReport
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nycklar
        public int EmployeeId { get; set; }
        public int BookingId { get; set; }

        // Arbetstid
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BreakMinutes { get; set; }

        // Beskrivning av utfört arbete
        public string Description { get; set; } = string.Empty;

        // När tidrapporten skapades
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigationsegenskaper
        public Employee Employee { get; set; } = null!;
        public Booking Booking { get; set; } = null!;
    }
}
