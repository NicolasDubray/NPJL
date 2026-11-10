using NPJL.Application.DTOs.Bookings;
using NPJL.Application.DTOs.Customers;
using NPJL.Application.DTOs.Invoices;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

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

                PaymentDetails = [..customerWithDetails.PaymentDetails //[..] är samma som .ToList()
                .Select(p => new PaymentDetailsDto
                {
                    PaymentMethod = p.PaymentMethod,
                    PaymentReference = p.PaymentReference,
                    IsActive = p.IsActive,

                })],

                Bookings = [..customerWithDetails.Bookings
                .Select(b => new BookingDto
                {
                    Id =b.Id,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })],

                Invoices = [.. customerWithDetails.Invoices
                .Where(i=> i.Status == Domain.Enums.InvoiceStatus.Sent || i.Status == InvoiceStatus.Paid || i.Status == InvoiceStatus.Expired)
                .Select(i=>  new InvoiceDto
                {
                    OCR = i.OCR,
                    Amount = i.InvoiceBookings.Sum(ib=> ib.Booking.BookingServices.Sum(bs=> bs.PriceAtBooking)),
                    SendDate = i.SendDate,
                    ExpiryDate = i.ExpiryDate,
                    Status = i.Status.ToString()

                })]

            };




        }
    }
}