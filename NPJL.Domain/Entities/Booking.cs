using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Enums;


namespace NPJL.Domain.Entities
{
    public class Booking
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nyckel till Customer
        public int CustomerId { get; set; }

        // Bokningens tidsperiod
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Bokningens status
        public BookingStatus Status { get; set; } = BookingStatus.Received;

        // Navigationsegenskaper
        public Customer Customer { get; set; } = null!;

        public ICollection<TimeReport> TimeReports { get; set; }
            = new List<TimeReport>();

        public ICollection<BookingService> BookingServices { get; set; }
            = new List<BookingService>();

        public ICollection<BookingTeam> BookingTeams { get; set; }
            = new List<BookingTeam>();

        public InvoiceBooking? InvoiceBooking { get; set; }
    }
}
