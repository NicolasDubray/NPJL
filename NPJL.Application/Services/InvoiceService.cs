using NPJL.Application.DTOs.Invoices;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

namespace NPJL.Application.Services;

public class InvoiceService(IInvoiceRepository _invoiceRepository)
{
    public async Task<List<InvoiceDto>> GetByCustomerIdAsync(int customerId)
    {
        var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);

        return invoices
            .Where(i => i.Status == InvoiceStatus.Sent
                     || i.Status == InvoiceStatus.Paid
                     || i.Status == InvoiceStatus.Expired)
            .Select(i => new InvoiceDto
            {
                OCR = i.OCR,
                Amount = CalculateAmount(i),
                SendDate = i.SendDate,
                ExpiryDate = i.ExpiryDate,
                Status = i.Status.ToString()
            })
            .ToList();
    }

    private static decimal CalculateAmount(Invoice invoice)
    {
        return invoice.InvoiceBookings
            .Sum(ib => ib.Booking.BookingServices.Sum(bs => bs.PriceAtBooking));
    }
}