
using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    // Repository för hantering av anställdas frånvaro.
    // Implementerar IAbsentismRepository och dess CRUD-operationer.
    public class AbsentismRepository : IAbsentismRepository
    {
        private readonly ApplicationDbContext _context;

        public AbsentismRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga frånvaroposter.
        public async Task<IReadOnlyList<Absentism>> GetAllAsync()
        {
            return await _context.Absentisms
                .AsNoTracking()
                .ToListAsync();
        }

        // Hämtar en frånvaropost via ID.
        public async Task<Absentism?> GetByIdAsync(int id)
        {
            return await _context.Absentisms
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Hämtar samtliga frånvaroposter för en anställd.
        public async Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Absentisms
                .AsNoTracking()
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }

        // Hämtar frånvaroposter som överlappar ett angivet datumintervall.
        public async Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.Absentisms
                .AsNoTracking()
                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.StartTime <= endDate &&
                    a.EndTime >= startDate)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        // Registrerar en ny frånvaropost.
        public async Task<Absentism> CreateAsync(Absentism absentism)
        {
            await _context.Absentisms.AddAsync(absentism);

            return absentism;
        }

        // Uppdaterar en befintlig frånvaropost.
        public Task UpdateAsync(Absentism absentism)
        {
            _context.Absentisms.Update(absentism);

            return Task.CompletedTask;
        }

        // Tar bort en frånvaropost via ID.
        public async Task DeleteAsync(int id)
        {
            var absentism = await _context.Absentisms.FindAsync(id);

            if (absentism != null)
            {
                _context.Absentisms.Remove(absentism);
            }
        }
        // Sparar ändringar i databasen.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
