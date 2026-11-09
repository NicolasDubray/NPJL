using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    
    
        public interface IAbsentismRepository : IRepository<Absentism>
        {
            // Hämtar samtliga frånvaroposter för en anställd.
            Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(int employeeId);
        
        Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate);
    }
}
