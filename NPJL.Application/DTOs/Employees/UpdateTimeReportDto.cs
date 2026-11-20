
namespace NPJL.Application.DTOs.Employees
{
    // DTO för uppdatering av en befintlig tidrapport.
    public sealed record UpdateTimeReportDto
    {
        public int BookingId { get; init; }

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public int BreakMinutes { get; init; }

        public string Description { get; init; } = string.Empty;
    }
}
