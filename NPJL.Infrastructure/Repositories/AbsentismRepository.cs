using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class AbsentismRepository(ApplicationDbContext context)
        : Repository<Absentism>(context), IAbsentismRepository
    {
        public Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}