
using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    public class BankInfoRepository : IBankInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public BankInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar samtliga bankuppgifter.
        public async Task<IReadOnlyList<BankInfo>> GetAllAsync()
        {
            return await _context.BankInfos
                .AsNoTracking()
                .ToListAsync();
        }

        // Hämtar bankuppgifter via ID.
        public async Task<BankInfo?> GetByIdAsync(int id)
        {
            return await _context.BankInfos
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        // Hämtar samtliga bankuppgifter för en anställd.
        public async Task<IReadOnlyList<BankInfo>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return await _context.BankInfos
                .AsNoTracking()
                .Where(b => b.EmployeeId == employeeId)
                .ToListAsync();
        }

        // Skapar nya bankuppgifter.
        public async Task<BankInfo> CreateAsync(BankInfo entity)
        {
            await _context.BankInfos.AddAsync(entity);

            return entity;
        }

        // Uppdaterar befintliga bankuppgifter.
        public Task UpdateAsync(BankInfo entity)
        {
            _context.BankInfos.Update(entity);

            return Task.CompletedTask;
        }

        // Tar bort bankuppgifter via ID.
        public async Task DeleteAsync(int id)
        {
            var bankInfo = await _context.BankInfos
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bankInfo != null)
            {
                _context.BankInfos.Remove(bankInfo);
            }
        }

        // Sparar genomförda ändringar i databasen.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}