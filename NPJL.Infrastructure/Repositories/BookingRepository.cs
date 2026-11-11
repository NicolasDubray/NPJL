using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class BookingRepository(ApplicationDbContext context)
        : Repository<Booking>(context), IBookingRepository
    {
        public async Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(int customerId)
            => await Context.Bookings
                .Where(b => b.CustomerId == customerId).ToListAsync();
    }
}