
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Absentisms
{
    // DTO för att överföra information om en frånvaropost.
    public sealed record AbsentismDto
    {
        public int Id { get; init; }

        public int EmployeeId { get; init; }

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public AbsentismType Type { get; init; }
    }
}
