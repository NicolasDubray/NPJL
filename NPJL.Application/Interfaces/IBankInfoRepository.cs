using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    public interface IBankInfoRepository : IRepository<BankInfo>
    {

        // Hämtar samtliga bankuppgifter för en anställd.
        Task<IReadOnlyList<BankInfo>> GetByEmployeeIdAsync(int employeeId);
    }
}
