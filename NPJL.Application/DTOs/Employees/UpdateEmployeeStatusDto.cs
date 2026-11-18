
namespace NPJL.Application.DTOs.Employees
{
    // DTO för att uppdatera en anställds aktiva status.
    public sealed record UpdateEmployeeStatusDto
    {
        public bool IsActive { get; init; }
    }
}
