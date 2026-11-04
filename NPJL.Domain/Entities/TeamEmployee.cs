using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class TeamEmployee
    {
        // Sammansatt primärnyckel
        public int TeamId { get; set; }
        public int EmployeeId { get; set; }

        // Navigationsegenskaper
        public Team Team { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}
