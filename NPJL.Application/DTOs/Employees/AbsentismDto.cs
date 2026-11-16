using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa frånvarohistorik för en anställd.
    public sealed record AbsentismDto
    {
        public int Id { get; init; }

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public AbsentismType Type { get; init; }
    }
}