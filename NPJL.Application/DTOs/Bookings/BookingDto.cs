namespace NPJL.Application.DTOs.Bookings
{
    public sealed record BookingDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = String.Empty;
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string Status { get; init; } = String.Empty;
    }
}
