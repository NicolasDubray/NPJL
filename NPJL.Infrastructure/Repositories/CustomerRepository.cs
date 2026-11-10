using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.ContactInfos)
                .Include(c => c.PaymentDetails)
                .Include(c => c.Bookings)
                .Include(c => c.Invoices)
                .ThenInclude(i => i.InvoiceBookings)
                .ThenInclude(ib => ib.Booking)
                .ThenInclude(b => b.BookingServices)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c =>
                    c.ContactInfos.Any(ci => ci.Email == email));
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);

            return customer;
        }

        public Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);

            return Task.CompletedTask;
        }
        public async Task RemoveAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }
        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.ContactInfos)
                .Include(c => c.PaymentDetails)
                .ToListAsync();
        }

        public async Task RemoveAsync()
        {

        }
        // Tar bort en kund via kundens ID.
        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers
                .FindAsync(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }
    }

}