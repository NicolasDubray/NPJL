using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;


namespace NPJL.Application.Interfaces
{
    // Repository för hantering av tidrapporter.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface ITimeReportRepository : IRepository<TimeReport>
    {
        // Hämtar tidrapporter för en viss anställd.
        Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(int employeeId);

        // Hämtar tidrapporter för en viss kund.
        Task<IReadOnlyList<TimeReport>> GetByCustomerIdAsync(int customerId);

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAndDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);
    }
}
