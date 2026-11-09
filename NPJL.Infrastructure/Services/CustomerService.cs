using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av kunder.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class CustomerService : GenericService<Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            ICustomerRepository repository)
            : base(repository)
        {
            _customerRepository = repository;
        }

        // Hämtar en kund via kundens e-postadress.
        public Task<Customer?> GetByEmailAsync(string email)
        {
            return _customerRepository.GetByEmailAsync(email);
        }
    }
}