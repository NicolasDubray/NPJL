using NPJL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NPJL.Infrastructure.Persistence;
using NPJL.Application.Interfaces;

namespace NPJL.Infrastructure.Repositories
{
    // Repository för hantering av tidrapporter.
    // Implementerar ITimeReportRepository och dess CRUD-operationer.
    public class TimeReportRepository(ApplicationDbContext context)
        : ITimeReportRepository
    {
        // Hämtar alla tidrapporter.
        public async Task<IReadOnlyList<TimeReport>> GetAllAsync()
        {
            return await context.TimeReports
                .AsNoTracking()
                .ToListAsync();
        }

        // Hämtar en tidrapport via ID.
        public async Task<TimeReport?> GetByIdAsync(int id)
        {
            return await context.TimeReports
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        // Hämtar tidrapporter för en viss anställd.
        public async Task<IReadOnlyList<TimeReport>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return await context.TimeReports
                .AsNoTracking()
                .Where(tr => tr.EmployeeId == employeeId)
                .OrderByDescending(tr => tr.StartTime)
                .ToListAsync();
        }

        // Hämtar tidrapporter för en viss kund.
        public async Task<IReadOnlyList<TimeReport>> GetByCustomerIdAsync(
            int customerId)
        {
            return await context.TimeReports
                .AsNoTracking()
                .Where(tr => tr.Booking.CustomerId == customerId)
                .OrderByDescending(tr => tr.StartTime)
                .ToListAsync();
        }

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        public async Task<IReadOnlyList<TimeReport>>
            GetByEmployeeIdAndDateRangeAsync(
                int employeeId,
                DateTime startDate,
                DateTime endDate)
        {
            return await context.TimeReports
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
            await context.TimeReports.AddAsync(timeReport);

            return timeReport;
        }

        // Uppdaterar en befintlig tidrapport.
        public Task UpdateAsync(TimeReport timeReport)
        {
            context.TimeReports.Update(timeReport);

            return Task.CompletedTask;
        }

        // Tar bort en tidrapport via ID.
        public async Task DeleteAsync(int id)
        {
            var timeReport = await context.TimeReports.FindAsync(id);

            if (timeReport != null)
            {
                context.TimeReports.Remove(timeReport);
            }
        }

        // Sparar ändringar i databasen.
        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(
                    $"Database error: {ex.InnerException?.Message}");

                throw;
            }
        }
    }
}