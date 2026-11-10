using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Domain.Entities
{
    public class Salary
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nyckel till Employee
        public int EmployeeId { get; set; }

        // Löneinformation
        public decimal Amount { get; set; }

        // Lönens giltighetsperiod
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        // När löneuppgiften skapades
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigationsegenskap
        public Employee Employee { get; set; } = null!;
    }
}