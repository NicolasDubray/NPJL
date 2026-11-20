using NPJL.Application.DTOs.Employees;

namespace NPJL.Application.Interfaces
{
    // Interface för applikationslogiken kring tidrapporter.
    public interface ITimeReportService
    {
        // Hämtar tidrapporter för en viss anställd.
        Task<IReadOnlyList<TimeReportDto>> GetByEmployeeIdAsync(int employeeId);

        // Hämtar tidrapporter för en viss kund.
        Task<IReadOnlyList<TimeReportDto>> GetByCustomerIdAsync(int customerId);

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        Task<IReadOnlyList<TimeReportDto>> GetByEmployeeIdAndDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);
        // Registrerar en ny tidrapport för en anställd.
        Task<TimeReportDto> CreateAsync(
            int employeeId,
            CreateTimeReportDto request);
        // Uppdaterar en befintlig tidrapport.
        Task<TimeReportDto> UpdateAsync(
            int timeReportId,
            int employeeId,
            UpdateTimeReportDto request);
        // Tar bort en befintlig tidrapport.
        Task DeleteAsync(int timeReportId);
        // Hämtar en enskild tidrapport via ID.
        Task<TimeReportDto?> GetByIdAsync(int timeReportId);
    }
}
