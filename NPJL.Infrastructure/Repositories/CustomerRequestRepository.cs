using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories;

public class CustomerRequestRepository(ApplicationDbContext context) : Repository<CustomerRequest>(context), ICustomerRequestRepository
{
    public override async Task<CustomerRequest> CreateAsync(CustomerRequest entity)
    {
        await Context.CustomerRequests.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity;
    }
}
