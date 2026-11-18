
namespace NPJL.Application.DTOs.Employees
{
    // DTO för registrering av en ny tidrapport.
    public sealed record CreateTimeReportDto
    {
        public int BookingId { get; init; }

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public int BreakMinutes { get; init; }

        public string Description { get; init; } = string.Empty;
    }
}
