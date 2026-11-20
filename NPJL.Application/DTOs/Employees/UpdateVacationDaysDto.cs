
namespace NPJL.Application.DTOs.Employees
{
    // DTO för att uppdatera en anställds semesterdagar.
    public sealed record UpdateVacationDaysDto
    {
        public int VacationDays { get; init; }
    }
}
