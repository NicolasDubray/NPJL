using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;


namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställdas tidrapporter.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class TimeReportService : GenericService<TimeReport>
    {
        private readonly ITimeReportRepository _timeReportRepository;

        public TimeReportService(
            ITimeReportRepository repository)
            : base(repository)
        {
            _timeReportRepository = repository;
        }

        // Hämtar tidrapporter för en viss anställd.
        public Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _timeReportRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Hämtar tidrapporter för en viss kund.
        public Task<IReadOnlyList<TimeReport>> GetByCustomerIdAsync(
            int customerId)
        {
            return _timeReportRepository.GetByCustomerIdAsync(customerId);
        }

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        public Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAndDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return _timeReportRepository.GetByEmployeeIdAndDateRangeAsync(
                employeeId,
                startDate,
                endDate);
        }
    }
}