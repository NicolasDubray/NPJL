using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;


namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställda.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class EmployeeService : GenericService<Employee>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(
            IEmployeeRepository repository)
            : base(repository)
        {
            _employeeRepository = repository;
        }

        // Hämtar en anställd via personnummer.
        public Task<Employee?> GetBySSNAsync(string ssn)
        {
            return _employeeRepository.GetBySSNAsync(ssn);
        }
    }
}