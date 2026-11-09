using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställdas bankuppgifter.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class BankInfoService : GenericService<BankInfo>
    {
        private readonly IBankInfoRepository _bankInfoRepository;

        public BankInfoService(
            IBankInfoRepository repository)
            : base(repository)
        {
            _bankInfoRepository = repository;
        }

        // Hämtar samtliga bankuppgifter för en anställd.
        public Task<IReadOnlyList<BankInfo>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _bankInfoRepository.GetByEmployeeIdAsync(employeeId);
        }
    }
}