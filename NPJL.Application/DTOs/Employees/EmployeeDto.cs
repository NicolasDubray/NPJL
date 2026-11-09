using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{
    public sealed record EmployeeDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = String.Empty;
        public string LastName { get; init; } = String.Empty;
        public bool IsCompany { get; init; }
    }
}
