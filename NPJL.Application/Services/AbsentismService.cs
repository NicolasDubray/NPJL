
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Application.DTOs.Absentisms;


namespace NPJL.Application.Services
{
    // Service för hantering av anställdas frånvaro.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class AbsentismService : GenericService<Absentism>
    {
        private readonly IAbsentismRepository _absentismRepository;

        public AbsentismService(IAbsentismRepository repository)
            : base(repository)
        {
            _absentismRepository = repository;
        }

        // Hämtar samtliga frånvaroposter för en anställd.
        public Task<IReadOnlyList<Absentism>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _absentismRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Hämtar frånvaroposter för en anställd inom ett datumintervall.
        public Task<IReadOnlyList<Absentism>> GetByDateRangeAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            return _absentismRepository.GetByDateRangeAsync(
                employeeId,
                startDate,
                endDate);
        }
        // Hämtar samtliga frånvaroposter för en anställd som DTO.
        public async Task<IReadOnlyList<AbsentismDto>> GetByEmployeeIdDetailsAsync(
            int employeeId)
        {
            var absences = await _absentismRepository.GetByEmployeeIdAsync(employeeId);

            return absences.Select(MapToDto).ToList();
        }

        // Hämtar frånvaroposter inom ett datumintervall som DTO.
        public async Task<IReadOnlyList<AbsentismDto>> GetByDateRangeDetailsAsync(
            int employeeId,
            DateTime startDate,
            DateTime endDate)
        {
            var absences = await _absentismRepository.GetByDateRangeAsync(
                employeeId,
                startDate,
                endDate);

            return absences.Select(MapToDto).ToList();
        }

        // Omvandlar Absentism-entiteten till AbsentismDto.
        private static AbsentismDto MapToDto(Absentism absentism)
        {
            return new AbsentismDto
            {
                Id = absentism.Id,
                EmployeeId = absentism.EmployeeId,
                StartTime = absentism.StartTime,
                EndTime = absentism.EndTime,
                Type = absentism.Type
            };
        }

        // Registrerar en ny frånvaropost.
        public async Task<AbsentismDto> CreateAsync(
            int employeeId,
            CreateAbsentismDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeId));
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException(
                    "Sluttiden måste vara senare än starttiden.");
            }

            var absentism = new Absentism
            {
                EmployeeId = employeeId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Type = request.Type
            };

            await _absentismRepository.CreateAsync(absentism);

            // Sparar ändringen i databasen.
            // Repositoryt ansvarar inte självt för SaveChanges.
            await _absentismRepository.SaveChangesAsync();

            return MapToDto(absentism);
        }

        // Uppdaterar en befintlig frånvaropost.
        public async Task<AbsentismDto?> UpdateAsync(
            int id,
            UpdateAbsentismDto request)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException(
                    "Sluttiden måste vara senare än starttiden.");
            }

            var absentism = await _absentismRepository.GetByIdAsync(id);

            if (absentism == null)
            {
                return null;
            }

            absentism.StartTime = request.StartTime;
            absentism.EndTime = request.EndTime;
            absentism.Type = request.Type;

            await _absentismRepository.UpdateAsync(absentism);
            await _absentismRepository.SaveChangesAsync();

            return MapToDto(absentism);
        }
        // Tar bort en frånvaropost.
        // Anropas endast av administratörer via API:t.
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var absentism = await _absentismRepository.GetByIdAsync(id);

            if (absentism == null)
                return false;

            await _absentismRepository.DeleteAsync(id);
            await _absentismRepository.SaveChangesAsync();

            return true;
        }


    }
}
