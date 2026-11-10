
using NPJL.Application.DTOs.Bookings;
using NPJL.Application.DTOs.Invoices;

namespace NPJL.Application.DTOs.Customers
{
    public sealed record CustomerDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = String.Empty;
        public ContactInfoDto? ContactInfo { get; init; }
        public List<BookingDto>? Bookings { get; init; }
        public List<InvoiceDto>? Invoices { get; init; }
        public List<PaymentDetailsDto>? PaymentDetails { get; init; }
    }
}
