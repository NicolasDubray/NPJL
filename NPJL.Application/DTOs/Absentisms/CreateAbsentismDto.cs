using System.ComponentModel.DataAnnotations;
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Absentisms
{
    // DTO för att registrera frånvaro för en anställd.
    public sealed record CreateAbsentismDto
    {
        [Required]
        public DateTime StartTime { get; init; }

        [Required]
        public DateTime EndTime { get; init; }

        [Required]
        public AbsentismType Type { get; init; }
    }
}