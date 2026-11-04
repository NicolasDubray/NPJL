using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Koppling till kund eller anställd
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        // Navigationsegenskaper
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
    }
}
