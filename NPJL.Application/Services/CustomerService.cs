using NPJL.Application.DTOs.Authentication;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Kontrollerar om e-postadressen redan används
        public async Task<bool> EmailExistsAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);

            return customer != null;
        }
        // Hämtar en kund med tillhörande kontakt- och betalningsuppgifter.
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }
    }
}