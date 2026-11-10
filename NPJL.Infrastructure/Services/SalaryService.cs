using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställdas löneuppgifter.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class SalaryService : GenericService<Salary>
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryService(
            ISalaryRepository repository)
            : base(repository)
        {
            _salaryRepository = repository;
        }

        // Hämtar samtliga löneuppgifter för en anställd.
        // Inkluderar både aktuell lön och tidigare löner.
        public Task<IReadOnlyList<Salary>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _salaryRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Hämtar den aktuella lönen för en anställd.
        // Returnerar null om ingen aktuell lön finns.
        public Task<Salary?> GetCurrentByEmployeeIdAsync(
            int employeeId)
        {
            return _salaryRepository.GetCurrentByEmployeeIdAsync(employeeId);
        }
    }
}
