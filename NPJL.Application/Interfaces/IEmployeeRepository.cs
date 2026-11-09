using NPJL.Application.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.DTOs.Employees;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av anställda.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Hämtar en anställd via personnummer.
        Task<Employee?> GetBySSNAsync(string ssn);
    }
}
