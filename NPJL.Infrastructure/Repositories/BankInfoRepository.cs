using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class BankInfoRepository(ApplicationDbContext context) : Repository<BankInfo>(context), IBankInfoRepository
    {
        public Task<IReadOnlyList<BankInfo>> GetByEmployeeIdAsync(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}