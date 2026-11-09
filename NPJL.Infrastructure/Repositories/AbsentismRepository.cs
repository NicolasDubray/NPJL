using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Infrastructure.Repositories
{
    public class AbsentismRepository
    {
        public async Task<IReadOnlyList<Absentism>> GetAllAsync()
        {
            // Implementera logik för att hämta alla frånvaroposter från databasen.
            throw new NotImplementedException();
        }
        public async Task<Absentism?> GetByIdAsync(int id)
        {
            // Implementera logik för att hämta en frånvaropost via dess ID från databasen.
            throw new NotImplementedException();
        }
        public async Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(int employeeId)
        {
            // Implementera logik för att hämta frånvaroposter för en viss anställd från databasen.
            throw new NotImplementedException();
        }
        public async Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            // Implementera logik för att hämta frånvaroposter inom ett angivet datumintervall från databasen.
            throw new NotImplementedException();
        }
        public async Task<Absentism> CreateAsync(Absentism absentism)
        {
            // Implementera logik för att registrera en ny frånvaropost i databasen.
            throw new NotImplementedException();
        }
        public async Task UpdateAsync(Absentism absentism)
        {
            // Implementera logik för att uppdatera en befintlig frånvaropost i databasen.
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(int id)
        {
            // Implementera logik för att ta bort en frånvaropost via dess ID från databasen.
            throw new NotImplementedException();
        }
        
    }
}
