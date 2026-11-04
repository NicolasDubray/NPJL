using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Enums;

namespace NPJL.Domain.Entities
{
    public class Invoice
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nyckel till Customer
        public int CustomerId { get; set; }

        // Fakturauppgifter
        public string OCR { get; set; } = string.Empty;
        public DateTime? SendDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Fakturans status
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        // Administratörens godkännande
        public string? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }

        // Navigationsegenskaper
        public Customer Customer { get; set; } = null!;

        public ICollection<InvoiceBooking> InvoiceBookings { get; set; }
            = new List<InvoiceBooking>();
    }
}
