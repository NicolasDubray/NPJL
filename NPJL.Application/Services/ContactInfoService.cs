using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services
{
    // Service for managing employee contact information.
    public class ContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;

        public ContactInfoService(IContactInfoRepository contactInfoRepository)
        {
            _contactInfoRepository = contactInfoRepository;
        }

        // Retrieves contact information belonging to an employee.
        public async Task<IReadOnlyList<ContactInfoDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            var contactInfos = await _contactInfoRepository.GetByEmployeeIdAsync(
                employeeId);

            return contactInfos.Select(c => new ContactInfoDto
            {
                Id = c.Id,
                Address = c.Address,
                Telephone = c.Telephone,
                Email = c.Email
            }).ToList();
        }

        // Creates contact information for an employee.
        public async Task<ContactInfo> CreateAsync(
            int employeeId,
            CreateContactInfoDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            var contactInfo = new ContactInfo
            {
                EmployeeId = employeeId,
                CustomerId = null,
                Address = request.Address.Trim(),
                Telephone = request.Telephone.Trim(),
                Email = request.Email.Trim()
            };

            await _contactInfoRepository.CreateAsync(contactInfo);
            await _contactInfoRepository.SaveChangesAsync();

            return contactInfo;
        }

        // Updates existing contact information belonging to an employee.
        public async Task<bool> UpdateAsync(
            int employeeId,
            int contactInfoId,
            UpdateContactInfoDto request)
        {
            if (employeeId <= 0 || contactInfoId <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            var contactInfo = await _contactInfoRepository.GetByIdAsync(
                contactInfoId);

            if (contactInfo == null ||
                contactInfo.EmployeeId != employeeId ||
                contactInfo.CustomerId != null)
            {
                return false;
            }

            contactInfo.Address = request.Address.Trim();
            contactInfo.Telephone = request.Telephone.Trim();
            contactInfo.Email = request.Email.Trim();

            await _contactInfoRepository.UpdateAsync(contactInfo);
            await _contactInfoRepository.SaveChangesAsync();

            return true;
        }
    }
}