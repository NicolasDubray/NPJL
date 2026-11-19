using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class InvoiceRepository(ApplicationDbContext context) : Repository<Invoice>(context), IInvoiceRepository
    {
        public async Task<IReadOnlyList<Invoice>> GetByCustomerIdAsync(int customerId)
            => await Context.Invoices
                .Where(i => i.CustomerId == customerId)
                .Include(i => i.InvoiceBookings)
                    .ThenInclude(ib => ib.Booking)
                    .ThenInclude(b => b.BookingServices)
                .ToListAsync();

        public Task<IReadOnlyList<Invoice>> GetByStatusAsync(InvoiceStatus status)
        {
            throw new NotImplementedException();
        }
    }
}