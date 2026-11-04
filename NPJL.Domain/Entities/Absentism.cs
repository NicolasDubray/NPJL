using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Enums;


namespace NPJL.Domain.Entities
{
    public class Absentism
    {
        // Primärnyckel
        public int Id { get; set; }

        // Främmande nyckel till Employee
        public int EmployeeId { get; set; }

        // Frånvaroperiod
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Typ av frånvaro
        public AbsentismType Type { get; set; }

        // Navigationsegenskap
        public Employee Employee { get; set; } = null!;
    }
}
