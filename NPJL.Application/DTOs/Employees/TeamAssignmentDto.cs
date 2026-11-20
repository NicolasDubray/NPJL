using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa en anställds Team- och ordertillhörighet.
    public sealed record TeamAssignmentDto
    {
        public int TeamId { get; init; }

        public int BookingId { get; init; }

        public DateTime BookingStartTime { get; init; }

        public DateTime BookingEndTime { get; init; }

        public string BookingStatus { get; init; } = string.Empty;

        public int CustomerId { get; init; }

        public string CustomerName { get; init; } = string.Empty;
    }
}
