using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    // Repository för hantering av anställdas löneuppgifter.
    public class SalaryRepository : ISalaryRepository
    {
        private readonly ApplicationDbContext _context;

        public SalaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga löneuppgifter.
        public async Task<IReadOnlyList<Salary>> GetAllAsync()
        {
            return await _context.Salaries
                .ToListAsync();
        }

        // Hämtar en lönepost via dess ID.
        public async Task<Salary?> GetByIdAsync(int id)
        {
            return await _context.Salaries
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Hämtar samtliga löneuppgifter för en anställd.
        // Sorterar efter startdatum så att lönehistoriken visas kronologiskt.
        public async Task<IReadOnlyList<Salary>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return await _context.Salaries
                .Where(s => s.EmployeeId == employeeId)
                .OrderBy(s => s.ValidFrom)
                .ToListAsync();
        }

        // Hämtar den aktuella lönen för en anställd.
        // En lönepost är aktuell om dagens datum ligger inom giltighetsperioden.
        public async Task<Salary?> GetCurrentByEmployeeIdAsync(
            int employeeId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Salaries
                .Where(s =>
                    s.EmployeeId == employeeId &&
                    s.ValidFrom <= today &&
                    (s.ValidTo == null || s.ValidTo >= today))
                .OrderByDescending(s => s.ValidFrom)
                .FirstOrDefaultAsync();
        }

        // Skapar en ny lönepost.
        public async Task<Salary> CreateAsync(Salary salary)
        {
            await _context.Salaries.AddAsync(salary);

            return salary;
        }

        // Uppdaterar en befintlig lönepost.
        public Task UpdateAsync(Salary salary)
        {
            _context.Salaries.Update(salary);

            return Task.CompletedTask;
        }

        // Tar bort en lönepost via dess ID.
        public async Task DeleteAsync(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);

            if (salary != null)
            {
                _context.Salaries.Remove(salary);
            }
        }
    }
}
