using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{

    namespace NPJL.Application.DTOs.Employees
    {
        // DTO för att överföra grundläggande personaluppgifter.
        // Innehåller inte känsliga uppgifter som bankinformation eller lönehistorik.
        public sealed record EmployeeDto
        {
            public int Id { get; init; }

            public string FirstName { get; init; } = string.Empty;

            public string LastName { get; init; } = string.Empty;

            public string SSN { get; init; } = string.Empty;

            public bool IsActive { get; init; }

            public int VacationDays { get; init; }
        }
    }

}
