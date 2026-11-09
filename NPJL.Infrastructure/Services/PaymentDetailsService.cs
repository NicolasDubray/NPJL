using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av kunders betalningsuppgifter.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class PaymentDetailsService : GenericService<PaymentDetails>
    {
        private readonly IPaymentDetailsRepository _paymentDetailsRepository;

        public PaymentDetailsService(
            IPaymentDetailsRepository repository)
            : base(repository)
        {
            _paymentDetailsRepository = repository;
        }

        // Hämtar samtliga betalningsuppgifter för en kund.
        public Task<IReadOnlyList<PaymentDetails>> GetByCustomerIdAsync(
            int customerId)
        {
            return _paymentDetailsRepository.GetByCustomerIdAsync(customerId);
        }
    }
}