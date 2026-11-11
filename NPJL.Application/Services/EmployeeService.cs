using NPJL.Application.DTOs.Employees;
using NPJL.Application.DTOs.Employees.NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
        // Hämtar samtliga aktiva anställda som DTO.
        public async Task<IReadOnlyList<EmployeeDto>> GetAllActiveDetailsAsync()
        {
            var employees = await _employeeRepository.GetAllActiveAsync();

            return employees.Select(MapToEmployeeDto).ToList();
        }
    }
}
