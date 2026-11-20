using System.ComponentModel.DataAnnotations;
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Absentisms
{
    // DTO för att uppdatera en befintlig frånvaropost.
    public sealed record UpdateAbsentismDto
    {
        [Required]
        public DateTime StartTime { get; init; }

        [Required]
        public DateTime EndTime { get; init; }

        [Required]
        public AbsentismType Type { get; init; }
    }
}