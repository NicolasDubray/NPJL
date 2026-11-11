using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    // Repository för hantering av anställda.
    // Implementerar IEmployeeRepository och dess CRUD-operationer.
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga anställda.
        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .ToListAsync();
        }

        // Hämtar en anställd via dennes ID.
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Hämtar samtliga aktiva anställda.
        public async Task<IReadOnlyList<Employee>> GetAllActiveAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        // Hämtar en anställd via personnummer.
        public async Task<Employee?> GetBySSNAsync(string ssn)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.SSN == ssn);
        }

        // Registrerar en ny anställd.
        public async Task<Employee> CreateAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            return employee;
        }

        // Uppdaterar en befintlig anställd.
        public Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);

            return Task.CompletedTask;
        }

        // Tar bort en anställd via dennes ID.
        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
        }
    }
}
