using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av fakturor.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        // Hämtar samtliga fakturor som tillhör en viss kund.
        Task<IReadOnlyList<Invoice>> GetByCustomerIdAsync(int customerId);

        // Hämtar fakturor med en viss status.
        // Exempelvis Draft, PendingApproval eller Approved.
        Task<IReadOnlyList<Invoice>> GetByStatusAsync(InvoiceStatus status);
    }
}
