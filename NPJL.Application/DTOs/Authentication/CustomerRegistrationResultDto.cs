using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Authentication
{
    public class CustomerRegistrationResultDto
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;

        public int? CustomerId { get; set; }
    }
}