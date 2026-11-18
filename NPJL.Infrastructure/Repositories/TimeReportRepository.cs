
using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NPJL.Infrastructure.Persistence;
using NPJL.Application.Interfaces;

namespace NPJL.Infrastructure.Repositories
{
    // Repository för hantering av tidrapporter.
    // Implementerar ITimeReportRepository och dess CRUD-operationer.
    public class TimeReportRepository : ITimeReportRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga tidrapporter från databasen.
        public async Task<IReadOnlyList<TimeReport>> GetAllAsync()
        {
            return await _context.TimeReports
                .AsNoTracking()
                .ToListAsync();
        }

        // Hämtar en tidrapport via ID.
        public async Task<TimeReport?> GetByIdAsync(int id)
        {
            return await _context.TimeReports
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        // Hämtar tidrapporter för en viss anställd.
        public async Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return await _context.TimeReports
                .AsNoTracking()
                .Where(tr => tr.EmployeeId == employeeId)
                .OrderByDescending(tr => tr.StartTime)
                .ToListAsync();
        }

        // Hämtar tidrapporter för en viss kund.
        public async Task<IReadOnlyList<TimeReport>> GetByCustomerIdAsync(
            int customerId)
        {
            return await _context.TimeReports
                .AsNoTracking()
                .Where(tr => tr.Booking.CustomerId == customerId)
                .OrderByDescending(tr => tr.StartTime)
                .ToListAsync();
        }

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        public async Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAndDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.TimeReports
                .AsNoTracking()
                .Where(tr =>
                    tr.EmployeeId == employeeId &&
                    tr.StartTime <= endDate &&
                    tr.EndTime >= startDate)
                .OrderBy(tr => tr.StartTime)
                .ToListAsync();
        }

        // Registrerar en ny tidrapport.
        public async Task<TimeReport> CreateAsync(TimeReport timeReport)
        {
            await _context.TimeReports.AddAsync(timeReport);

            return timeReport;
        }

        // Uppdaterar en befintlig tidrapport.
        public Task UpdateAsync(TimeReport timeReport)
        {
            _context.TimeReports.Update(timeReport);

            return Task.CompletedTask;
        }

        // Tar bort en tidrapport via ID.
        public async Task DeleteAsync(int id)
        {
            var timeReport = await _context.TimeReports.FindAsync(id);

            if (timeReport != null)
            {
                _context.TimeReports.Remove(timeReport);
            }
        }

        // Sparar ändringar i databasen.
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
