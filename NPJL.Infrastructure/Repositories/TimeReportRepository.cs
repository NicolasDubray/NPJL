using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class TimeReportRepository(ApplicationDbContext context)
        : Repository<TimeReport>(context), ITimeReportRepository
    {
        public async Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(int employeeId)
            => await Context.TimeReports
                .Where(tr => tr.EmployeeId == employeeId)
                .ToListAsync();

        public Task<IReadOnlyList<TimeReport>> GetByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAndDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}