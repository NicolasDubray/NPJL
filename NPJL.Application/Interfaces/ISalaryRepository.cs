using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    // Repository för hantering av anställdas löneuppgifter.
    // Ärver gemensamma CRUD-operationer från IRepository.
    public interface ISalaryRepository : IRepository<Salary>
    {
        // Hämtar samtliga löneuppgifter för en anställd.
        // Inkluderar både aktuell lön och tidigare löner.
        Task<IReadOnlyList<Salary>> GetByEmployeeIdAsync(int employeeId);

        // Hämtar den aktuella lönen för en anställd.
        // Returnerar null om ingen aktuell lön finns.
        Task<Salary?> GetCurrentByEmployeeIdAsync(int employeeId);

        // Kontrollerar om en anställd redan har en löneperiod
        // som överlappar med det angivna datumintervallet.
        Task<bool> HasOverlappingPeriodAsync(
            int employeeId,
            DateTime validFrom,
            DateTime? validTo);

        // Sparar samtliga ändringar i databasen.
        Task SaveChangesAsync();
    }
}

