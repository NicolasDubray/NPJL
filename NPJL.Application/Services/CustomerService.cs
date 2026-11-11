using NPJL.Application.DTOs.Customers;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

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

        public async Task<CustomerDto> GetProfileByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email)
                ?? throw new Exception("Can not find customer");
            var customerWithDetails = await _customerRepository.GetByIdAsync(customer.Id)
                ?? throw new Exception("Can not find Customer");
            var contact = customerWithDetails.ContactInfos.FirstOrDefault();
            return new CustomerDto
            {
                Id = customerWithDetails.Id,
                Name = customerWithDetails.FirstName + " " + customerWithDetails.LastName,
                ContactInfo = contact == null ? null : new ContactInfoDto()
                {
                    Address = contact.Address,
                    Email = contact.Email,
                    Telephone = contact.Telephone
                },

            };

        }
    }
}