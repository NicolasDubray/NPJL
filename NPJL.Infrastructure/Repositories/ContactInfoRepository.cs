
using Microsoft.EntityFrameworkCore;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Repositories
{
    // Repository for managing employee contact information.
    public class ContactInfoRepository : IContactInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves contact information by its ID.
        public async Task<ContactInfo?> GetByIdAsync(int id)
        {
            return await _context.ContactInfos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Retrieves all contact information for a specific employee.
        public async Task<IReadOnlyList<ContactInfo>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return await _context.ContactInfos
                .AsNoTracking()
                .Where(c => c.EmployeeId == employeeId)
                .ToListAsync();
        }

        // Creates new contact information.
        public async Task<ContactInfo> CreateAsync(ContactInfo contactInfo)
        {
            await _context.ContactInfos.AddAsync(contactInfo);

            return contactInfo;
        }

        // Updates existing contact information.
        public Task UpdateAsync(ContactInfo contactInfo)
        {
            _context.ContactInfos.Update(contactInfo);

            return Task.CompletedTask;
        }

        // Deletes contact information by its ID.
        public async Task DeleteAsync(int id)
        {
            var contactInfo = await _context.ContactInfos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contactInfo != null)
            {
                _context.ContactInfos.Remove(contactInfo);
            }
        }

        // Saves pending changes to the database.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
