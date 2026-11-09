using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Repositories
{
    public interface EmployeeRepository
    {
        // Hämtar samtliga anställda.
        Task<IReadOnlyList<Employee>> GetAllAsync();

        // Hämtar en anställd via dennes ID.
        Task<Employee?> GetByIdAsync(int id);

        // Hämtar samtliga aktiva anställda.
        Task<IReadOnlyList<Employee>> GetAllActiveAsync();

        // Registrerar en ny anställd.
        Task<Employee> CreateAsync(Employee employee);

        // Uppdaterar en befintlig anställd.
        Task UpdateAsync(Employee employee);

        // Tar bort en anställd via dennes ID.
        Task DeleteAsync(int id);
    }
}
