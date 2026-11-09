using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;
using NPJL.Application.DTOs.Bookings;
using NPJL.Application.DTOs.Customers;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av kunder.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Hämtar en kund via kundens e-postadress.
        Task<Customer?> GetByEmailAsync(string email);
    }

}
