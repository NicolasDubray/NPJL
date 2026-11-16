using System;
using System.Collections.Generic;
using System.Text;



namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa detaljerad information om en anställd.
    // Känsliga löne- och bankuppgifter hanteras separat.
    public sealed record EmployeeDetailsDto
    {
        public int Id { get; init; }

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string SSN { get; init; } = string.Empty;

        public bool IsActive { get; init; }

        public int VacationDays { get; init; }

        // Kontaktuppgifter.
        public List<ContactInfoDto> ContactInfos { get; init; } = new();

        // Anställningshistorik.
        public List<EmploymentDto> Employments { get; init; } = new();
        // Salary history.
        public List<SalaryDto> Salaries { get; init; } = new();

        // Banking details.
        public List<BankInfoDto> BankInfos { get; init; } = new();
        // Frånvarohistorik.
        public List<AbsentismDto> Absentisms { get; init; } = new();

        // Tidrapporter.
        public List<TimeReportDto> TimeReports { get; init; } = new();

        // Team och tillhörande order.
        public List<TeamAssignmentDto> TeamAssignments { get; init; } = new();
    }
}
