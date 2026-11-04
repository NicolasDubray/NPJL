using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class Employment
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nyckel till Employee
        public int EmployeeId { get; set; }

        // Anställningsinformation
        public string HireType { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal NumWorkingHours { get; set; }

        // Navigationsegenskap
        public Employee Employee { get; set; } = null!;
    }
}
