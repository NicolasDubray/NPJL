
namespace NPJL.Application.DTOs.Employees
{
    // DTO for registering a new salary period.
    public sealed record CreateSalaryDto
    {
        public decimal Amount { get; init; }

        public DateTime ValidFrom { get; init; }

        public DateTime? ValidTo { get; init; }
    }
}
