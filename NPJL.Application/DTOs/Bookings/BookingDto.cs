using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Bookings
{
    public sealed record BookingDto
    {
        int Id { get; init; }
        string Title { get; init; } = String.Empty;
    }
}
