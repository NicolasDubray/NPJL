
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Hämtar samtliga anställda.
        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        // Hämtar samtliga aktiva anställda.
        public async Task<IReadOnlyList<Employee>> GetAllActiveAsync()
        {
            return await _employeeRepository.GetAllActiveAsync();
        }

        // Hämtar en anställd via dennes ID.
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        // Hämtar en anställd via personnummer.
        public async Task<Employee?> GetBySSNAsync(string ssn)
        {
            return await _employeeRepository.GetBySSNAsync(ssn);
        }

        // Hämtar samtliga anställda som DTO.
        public async Task<IReadOnlyList<EmployeeDto>> GetAllDetailsAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return employees.Select(MapToEmployeeDto).ToList();
        }

        // Hämtar en anställd via ID som DTO.
        public async Task<EmployeeDto?> GetDetailsByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            return employee == null ? null : MapToEmployeeDto(employee);
        }

        // Hämtar en anställd med tillhörande information som DTO.
        public async Task<EmployeeDetailsDto?> GetDetailsWithRelationsByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetWithDetailsByIdAsync(id);

            return employee == null ? null : MapToEmployeeDetailsDto(employee);
        }

        // Omvandlar Employee-entiteten till EmployeeDto.
        private static EmployeeDto MapToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                SSN = employee.SSN,
                IsActive = employee.IsActive,
                VacationDays = employee.VacationDays
            };
        }

        // Omvandlar Employee-entiteten till EmployeeDetailsDto.
        private static EmployeeDetailsDto MapToEmployeeDetailsDto(Employee employee)
        {
            return new EmployeeDetailsDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                SSN = employee.SSN,
                IsActive = employee.IsActive,
                VacationDays = employee.VacationDays,

                // Kontaktuppgifter.
                ContactInfos = employee.ContactInfos
                    .Select(c => new ContactInfoDto
                    {
                        Id = c.Id,
                        Address = c.Address,
                        Telephone = c.Telephone,
                        Email = c.Email
                    })
                    .ToList(),

                // Anställningshistorik.
                Employments = employee.Employments
                    .Select(e => new EmploymentDto
                    {
                        Id = e.Id,
                        HireType = e.HireType,
                        HireDate = e.HireDate,
                        Role = e.Role,
                        Department = e.Department,
                        NumWorkingHours = e.NumWorkingHours
                    })
                    .ToList(),

                // Salary history.
                Salaries = employee.Salaries
                    .Select(s => new SalaryDto
                    {
                        Id = s.Id,
                        Amount = s.Amount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo,
                        CreatedAt = s.CreatedAt
                    })
                    .ToList(),

                // Banking details.
                BankInfos = employee.BankInfos
                    .Select(b => new BankInfoDto
                    {
                        Id = b.Id,
                        AccountHolderName = b.AccountHolderName,
                        AccountHolderType = b.AccountHolderType.ToString(),
                        BankName = b.BankName,
                        BankAccountNumber = b.BankAccountNumber,
                        ClearingNumber = b.ClearingNumber
                    })
                    .ToList(),

                // Frånvarohistorik.
                Absentisms = employee.Absentisms
                    .Select(a => new AbsentismDto
                    {
                        Id = a.Id,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Type = a.Type
                    })
                    .ToList(),

                // Tidrapporter.
                TimeReports = employee.TimeReports
                    .Select(t => new TimeReportDto
                    {
                        Id = t.Id,
                        BookingId = t.BookingId,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        BreakMinutes = t.BreakMinutes,
                        Description = t.Description,
                        CreatedAt = t.CreatedAt
                    })
                    .ToList(),

                // Team och tillhörande order.
                TeamAssignments = employee.TeamEmployees
                    .SelectMany(te => te.Team.BookingTeams
                        .Select(bt => new TeamAssignmentDto
                        {
                            TeamId = te.TeamId,
                            BookingId = bt.BookingId,
                            BookingStartTime = bt.Booking.StartTime,
                            BookingEndTime = bt.Booking.EndTime,
                            BookingStatus = bt.Booking.Status.ToString(),
                            CustomerId = bt.Booking.CustomerId,

                            CustomerName = bt.Booking.Customer.IsCompany
                                ? bt.Booking.Customer.CompanyName ?? string.Empty
                                : $"{bt.Booking.Customer.FirstName} {bt.Booking.Customer.LastName}"
                        }))
                    .ToList()
            };
        }
        
        // Hämtar samtliga aktiva anställda som DTO.
        public async Task<IReadOnlyList<EmployeeDto>> GetAllActiveDetailsAsync()
        {
            var employees = await _employeeRepository.GetAllActiveAsync();

            return employees.Select(MapToEmployeeDto).ToList();
        }

        // Uppdaterar en anställds semesterdagar.
        public async Task<bool> UpdateVacationDaysAsync(
            int employeeId,
            UpdateVacationDaysDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            if (request.VacationDays < 0)
            {
                throw new ArgumentException(
                    "Vacation days cannot be negative.");
            }
            
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                return false;
            }

            employee.VacationDays = request.VacationDays;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return true;
        }

        // Updates an employee's active status.
        public async Task<bool> UpdateEmployeeStatusAsync(
            int employeeId,
            UpdateEmployeeStatusDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
            {
                return false;
            }

            employee.IsActive = request.IsActive;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return true;
        }


    }
}
