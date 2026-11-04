using System;
using System.Collections.Generic;
using System.Text;



namespace NPJL.Domain.Entities
{
    public class PaymentDetails
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        // Kundens valda betalmetod
        public string PaymentMethod { get; set; } = string.Empty;

        // Exempelvis en token eller referens från betalningsleverantören
        public string PaymentReference { get; set; } = string.Empty;

        // Anger om betalmetoden fortfarande är aktiv
        public bool IsActive { get; set; } = true;

        public Customer Customer { get; set; } = null!;
    }
}

