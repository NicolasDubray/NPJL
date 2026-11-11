using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class PaymentDetailsRepository(ApplicationDbContext context)
        : Repository<PaymentDetails>(context), IPaymentDetailsRepository
    {
        public async Task<IReadOnlyList<PaymentDetails>> GetByCustomerIdAsync(int customerId)
            => await Context.PaymentDetails
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
    }
}