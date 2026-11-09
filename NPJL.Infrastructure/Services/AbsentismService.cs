using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställdas frånvaro.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class AbsentismService : GenericService<Absentism>
    {
        private readonly IAbsentismRepository _absentismRepository;
        public AbsentismService(
            IRepository<Absentism> repository)
            : base(repository)
        {
        }
        // Hämtar samtliga frånvaroposter för en anställd.
        public Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _absentismRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Hämtar frånvaroposter för en anställd inom ett datumintervall.
        public Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return _absentismRepository.GetByDateRangeAsync(
                employeeId,
                startDate,
                endDate);
        }
    }
}
