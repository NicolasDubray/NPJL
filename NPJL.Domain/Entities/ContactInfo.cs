using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class ContactInfo
    {
        // Primärnyckel
        public int Id { get; set; }

        // Kontaktuppgifter
        public string Address { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Främmande nycklar
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }

        // Navigationsegenskaper
        public Employee? Employee { get; set; }
        public Customer? Customer { get; set; }
    }
}
