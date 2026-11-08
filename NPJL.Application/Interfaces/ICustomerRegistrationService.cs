using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.DTOs.Authentication;

namespace NPJL.Application.Interfaces
{
    public interface ICustomerRegistrationService
    {
        Task<CustomerRegistrationResultDto> RegisterAsync(
            CustomerRegistrationDto request);
    }
}