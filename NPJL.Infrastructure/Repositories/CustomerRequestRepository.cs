using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories;

public class CustomerRequestRepository(ApplicationDbContext context)
    : Repository<CustomerRequest>(context), ICustomerRequestRepository
{
    public override async Task<IReadOnlyList<CustomerRequest>> GetAllAsync()
    {
        var requests = await Context.CustomerRequests
            .AsNoTracking()
            .Include(r => r.Customer)
            .ToListAsync();

        var customerIds = requests
            .Where(r => r.Customer != null)
            .Select(r => r.CustomerId)
            .Distinct()
            .ToList();

        var contactInfos = await Context.ContactInfos
            .AsNoTracking()
            .Where(c => c.CustomerId.HasValue
                        && customerIds.Contains(c.CustomerId.Value))
            .ToListAsync();

        foreach (var request in requests)
        {
            if (request.Customer == null)
            {
                continue;
            }

            request.Customer.ContactInfos = contactInfos
                .Where(c => c.CustomerId == request.CustomerId)
                .ToList();
        }

        return requests;
    }

    public override async Task<CustomerRequest> CreateAsync(CustomerRequest entity)
    {
        await Context.CustomerRequests.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public override async Task UpdateAsync(CustomerRequest entity)
    {
        Context.CustomerRequests.Update(entity);
        await Context.SaveChangesAsync();
    }
}