using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.DTOs.Authentication;
using NPJL.Application.Interfaces;

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
    }
}