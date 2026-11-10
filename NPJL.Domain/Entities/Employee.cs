using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class Employee
    {
        // Primärnyckel
        public int Id { get; set; }

        // Personuppgifter
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;

        // Anställningsstatus
        public bool IsActive { get; set; } = true;
        public int VacationDays { get; set; }

        // Navigationsegenskaper
        public ICollection<Employment> Employments { get; set; }
            = new List<Employment>();

        public ICollection<BankInfo> BankInfos { get; set; }
            = new List<BankInfo>();

        public ICollection<Absentism> Absentisms { get; set; }
            = new List<Absentism>();

        public ICollection<TimeReport> TimeReports { get; set; }
            = new List<TimeReport>();

        public ICollection<ContactInfo> ContactInfos { get; set; }
            = new List<ContactInfo>();

        public ICollection<TeamEmployee> TeamEmployees { get; set; }
            = new List<TeamEmployee>();

        // Löneuppgifter
        public ICollection<Salary> Salaries { get; set; }
            = new List<Salary>();
    }
}
