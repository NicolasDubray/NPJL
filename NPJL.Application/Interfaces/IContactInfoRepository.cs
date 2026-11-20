
using NPJL.Domain.Entities;

namespace NPJL.Application.Interfaces
{
    // Repository for managing employee contact information.
    public interface IContactInfoRepository
    {
        // Retrieves contact information by its ID.
        Task<ContactInfo?> GetByIdAsync(int id);

        // Retrieves all contact information for a specific employee.
        Task<IReadOnlyList<ContactInfo>> GetByEmployeeIdAsync(int employeeId);

        // Creates new contact information.
        Task<ContactInfo> CreateAsync(ContactInfo contactInfo);

        // Updates existing contact information.
        Task UpdateAsync(ContactInfo contactInfo);

        // Deletes contact information by its ID.
        Task DeleteAsync(int id);

        // Saves pending changes to the database.
        Task SaveChangesAsync();
    }
}
