using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av fakturor.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class InvoiceService : GenericService<Invoice>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(
            IInvoiceRepository repository)
            : base(repository)
        {
            _invoiceRepository = repository;
        }

        // Hämtar samtliga fakturor som tillhör en viss kund.
        public Task<IReadOnlyList<Invoice>> GetByCustomerIdAsync(
            int customerId)
        {
            return _invoiceRepository.GetByCustomerIdAsync(customerId);
        }

        // Hämtar fakturor med en viss status.
        public Task<IReadOnlyList<Invoice>> GetByStatusAsync(
            InvoiceStatus status)
        {
            return _invoiceRepository.GetByStatusAsync(status);
        }
    }
}