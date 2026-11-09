using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;


namespace NPJL.Application.Interfaces
{
    // Repository för hantering av kunders betalningsuppgifter.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails>
    {
        // Hämtar samtliga betalningsuppgifter för en kund.
        // En kund kan exempelvis ha flera registrerade betalningsmetoder.
        Task<IReadOnlyList<PaymentDetails>> GetByCustomerIdAsync(int customerId);
    }
}
