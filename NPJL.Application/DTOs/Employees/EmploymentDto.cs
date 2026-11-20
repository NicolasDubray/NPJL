using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa anställningsinformation.
    public sealed record EmploymentDto
    {
        public int Id { get; init; }

        public string HireType { get; init; } = string.Empty;

        public DateTime HireDate { get; init; }

        public string Role { get; init; } = string.Empty;

        public string Department { get; init; } = string.Empty;

        // Avtalade arbetstimmar per vecka.
        public decimal NumWorkingHours { get; init; }
    }
}