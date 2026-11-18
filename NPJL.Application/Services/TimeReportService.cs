using System;
using System.Collections.Generic;
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services
{
    // Service för hantering av tidrapporter.
    public class TimeReportService : ITimeReportService
    {
        private readonly ITimeReportRepository _timeReportRepository;
        
        public TimeReportService(ITimeReportRepository timeReportRepository)
        {
            _timeReportRepository = timeReportRepository;
        }

        // Hämtar en enskild tidrapport via ID.
        public async Task<TimeReportDto?> GetByIdAsync(int timeReportId)
        {
            if (timeReportId <= 0)
            {
                throw new ArgumentException("Invalid time report ID.");
            }

            var timeReport = await _timeReportRepository
                .GetByIdAsync(timeReportId);
            
            if (timeReport == null)
            {
                return null;
            }

            return MapToDto(timeReport);
        }

        // Hämtar tidrapporter för en viss anställd.
        public async Task<IReadOnlyList<TimeReportDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            var timeReports = await _timeReportRepository
                .GetByEmployeeIdAsync(employeeId);

            return timeReports.Select(MapToDto).ToList();
        }

        // Hämtar tidrapporter för en viss kund.
        public async Task<IReadOnlyList<TimeReportDto>> GetByCustomerIdAsync(
            int customerId)
        {
            var timeReports = await _timeReportRepository
                .GetByCustomerIdAsync(customerId);

            return timeReports.Select(MapToDto).ToList();
        }

        // Hämtar tidrapporter för en anställd inom ett datumintervall.
        public async Task<IReadOnlyList<TimeReportDto>>
            GetByEmployeeIdAndDateRangeAsync(
                int employeeId,
                DateTime startDate,
                DateTime endDate)
        {
            var timeReports = await _timeReportRepository
                .GetByEmployeeIdAndDateRangeAsync(
                    employeeId,
                    startDate,
                    endDate);

            return timeReports.Select(MapToDto).ToList();
        }

        // Mappar en TimeReport-entitet till TimeReportDto.
        private static TimeReportDto MapToDto(TimeReport timeReport)
        {
            return new TimeReportDto
            {
                Id = timeReport.Id,
                BookingId = timeReport.BookingId,
                StartTime = timeReport.StartTime,
                EndTime = timeReport.EndTime,
                BreakMinutes = timeReport.BreakMinutes,
                Description = timeReport.Description,
                CreatedAt = timeReport.CreatedAt,
                EmployeeId = timeReport.EmployeeId,
            };
        }


        // Registrerar en ny tidrapport.
        public async Task<TimeReportDto> CreateAsync(
            int employeeId,
            CreateTimeReportDto request)
        {
            // Kontrollerar att anställdens ID är giltigt.
            if (employeeId <= 0)
            {
                throw new ArgumentException(
                    "Invalid employee ID.",
                    nameof(employeeId));
            }

            // Kontrollerar att bokningens ID är giltigt.
            // Kontroll av att bokningen existerar tillkommer senare.
            if (request.BookingId <= 0)
            {
                throw new ArgumentException(
                    "Invalid booking ID.",
                    nameof(request.BookingId));
            }

            // Sluttiden måste vara efter starttiden.
            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException(
                    "End time must be later than start time.");
            }

            // Beräknar hela tidsintervallet i minuter.
            var totalMinutes = (request.EndTime - request.StartTime).TotalMinutes;

            // Rasten får inte vara negativ eller längre än tidsintervallet.
            if (request.BreakMinutes < 0 ||
                request.BreakMinutes > totalMinutes)
            {
                throw new ArgumentException(
                    "Break time must be between zero and the total time interval.");
            }

            // Kontrollerar beskrivningens längd.
            if (request.Description?.Length > 1000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 1000 characters.");
            }

            var timeReport = new TimeReport
            {
                EmployeeId = employeeId,
                BookingId = request.BookingId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                BreakMinutes = request.BreakMinutes,
                Description = request.Description?.Trim() ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _timeReportRepository.CreateAsync(timeReport);
            await _timeReportRepository.SaveChangesAsync();

            return MapToDto(timeReport);
        }

        // Uppdaterar en befintlig tidrapport.
        public async Task<TimeReportDto> UpdateAsync(
            int timeReportId,
            int employeeId,
            UpdateTimeReportDto request)
        {
            // Kontrollerar att ID:n är giltiga.
            if (timeReportId <= 0 || employeeId <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            // Hämtar befintlig tidrapport.
            var timeReport = await _timeReportRepository
                .GetByIdAsync(timeReportId);

            if (timeReport == null)
            {
                throw new KeyNotFoundException("Time report not found.");
            }

            // Säkerställer att rapporten tillhör rätt anställd.
            if (timeReport.EmployeeId != employeeId)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to update this time report.");
            }

            // Kontrollerar boknings-ID.
            // Kontroll av att bokningen existerar tillkommer senare.
            if (request.BookingId <= 0)
            {
                throw new ArgumentException("Invalid booking ID.");
            }

            // Kontrollerar tidsintervallet.
            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException(
                    "End time must be later than start time.");
            }

            var totalMinutes =
                (request.EndTime - request.StartTime).TotalMinutes;

            // Kontrollerar rastens längd.
            if (request.BreakMinutes < 0 ||
                request.BreakMinutes > totalMinutes)
            {
                throw new ArgumentException(
                    "Break time must be between zero and the total time interval.");
            }

            // Kontrollerar beskrivningens längd.
            if (request.Description?.Length > 1000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 1000 characters.");
            }

            // Uppdaterar rapportens uppgifter.
            timeReport.BookingId = request.BookingId;
            timeReport.StartTime = request.StartTime;
            timeReport.EndTime = request.EndTime;
            timeReport.BreakMinutes = request.BreakMinutes;
            timeReport.Description =
                request.Description?.Trim() ?? string.Empty;

            await _timeReportRepository.UpdateAsync(timeReport);
            await _timeReportRepository.SaveChangesAsync();

            return MapToDto(timeReport);
        }

        // Tar bort en befintlig tidrapport.
        public async Task DeleteAsync(int timeReportId)
        {
            // Kontrollerar att ID är giltigt.
            if (timeReportId <= 0)
            {
                throw new ArgumentException("Invalid time report ID.");
            }

            // Kontrollerar att tidrapporten existerar.
            var timeReport = await _timeReportRepository
                .GetByIdAsync(timeReportId);

            if (timeReport == null)
            {
                throw new KeyNotFoundException("Time report not found.");
            }

            await _timeReportRepository.DeleteAsync(timeReportId);
            await _timeReportRepository.SaveChangesAsync();
        }




    }
}
