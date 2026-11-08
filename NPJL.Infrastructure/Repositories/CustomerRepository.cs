using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
