using System;
using System.Collections.Generic;
using System.Text;



namespace NPJL.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        // Används för privatpersoner
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Används för företagskunder
        public string? CompanyName { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsCompany { get; set; }

        public ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
        public ICollection<PaymentDetails> PaymentDetails { get; set; } = new List<PaymentDetails>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
