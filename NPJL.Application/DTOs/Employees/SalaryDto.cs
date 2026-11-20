
namespace NPJL.Application.DTOs.Employees
{
    // DTO for displaying an employee's salary information.
    public sealed record SalaryDto
    {
        public int Id { get; init; }

        public decimal Amount { get; init; }

        public DateTime ValidFrom { get; init; }

        public DateTime? ValidTo { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}
