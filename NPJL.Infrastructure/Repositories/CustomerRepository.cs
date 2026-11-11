using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext context)
        : Repository<Customer>(context), ICustomerRepository
    {
        public override async Task<Customer?> GetByIdAsync(int id)
        {
            return await Context.Customers
                .Include(c => c.ContactInfos)
                .Include(c => c.PaymentDetails)
                .Include(c => c.Bookings)
                .Include(c => c.Invoices)
                .ThenInclude(i => i.InvoiceBookings)
                .ThenInclude(ib => ib.Booking)
                .ThenInclude(b => b.BookingServices)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await Context.Customers
                .Include(c => c.ContactInfos)
                .Include(c => c.PaymentDetails)
                .ToListAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await Context.Customers
                .FirstOrDefaultAsync(c =>
                    c.ContactInfos.Any(ci => ci.Email == email));
        }
    }
}