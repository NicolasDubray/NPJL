using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByEmailAsync(string email);

        Task<Customer> CreateAsync(Customer customer);

        Task UpdateAsync(Customer customer);
    }
}
