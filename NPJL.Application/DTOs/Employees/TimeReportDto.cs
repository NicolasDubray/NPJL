using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa tidrapporter för en anställd.
    public sealed record TimeReportDto
    {
        public int Id { get; init; }

        public int BookingId { get; init; }

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public int BreakMinutes { get; init; }

        public string Description { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
    }
}