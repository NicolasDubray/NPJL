using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class TimeReportRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga tidrapporter från databasen.
        public async Task<IReadOnlyList<TimeReport>> GetAllAsync()
        {
            return await _context.TimeReports.ToListAsync();
        }
        public async Task<TimeReport?> GetByIdAsync(int id)
        {
            return await _context.TimeReports.FindAsync(id);
        }
        public async Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.TimeReports
                .Where(tr => tr.EmployeeId == employeeId)
                .ToListAsync();
        }
     

    }
}
