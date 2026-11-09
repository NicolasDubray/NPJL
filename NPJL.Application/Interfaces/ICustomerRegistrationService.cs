using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.DTOs.Authentication;

namespace NPJL.Application.Interfaces
{
    // Interface för registrering av nya kundkonton.
    public interface ICustomerRegistrationService
    {
        // Registrerar en ny kund och skapar tillhörande användarkonto.
        Task<CustomerRegistrationResultDto> RegisterAsync(
            CustomerRegistrationDto request);
    }
}
