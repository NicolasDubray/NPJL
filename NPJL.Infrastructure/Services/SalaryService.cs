using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;


namespace NPJL.Infrastructure.Services
{
    // Service för hantering av anställdas löneuppgifter.
    // Ärver grundläggande hämtningsmetoder från GenericService.
    public class SalaryService : GenericService<Salary>
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryService(
            ISalaryRepository repository)
            : base(repository)
        {
            _salaryRepository = repository;
        }

        // Hämtar samtliga löneuppgifter för en anställd.
        // Inkluderar både aktuell lön och tidigare löner.
        public Task<IReadOnlyList<Salary>> GetByEmployeeIdAsync(
            int employeeId)
        {
            return _salaryRepository.GetByEmployeeIdAsync(employeeId);
        }

        // Hämtar den aktuella lönen för en anställd.
        // Returnerar null om ingen aktuell lön finns.
        public Task<Salary?> GetCurrentByEmployeeIdAsync(
            int employeeId)
        {
            return _salaryRepository.GetCurrentByEmployeeIdAsync(employeeId);
        }

        // Registrerar en ny löneperiod för en anställd.
        // Avslutar föregående löneperiod om den fortfarande gäller.
        public async Task<Salary> RegisterSalaryChangeAsync(
            int employeeId,
            decimal amount,
            DateTime validFrom,
            DateTime? validTo = null)
        {
            // Grundläggande validering av indata.
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(employeeId),
                    "Anställdens ID måste vara större än noll.");
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Lönen måste vara större än noll.");
            }

            // Databasen lagrar giltighetsdatumen som date.
            // Därför tar vi bort eventuell tidsdel.
            validFrom = validFrom.Date;

            if (validTo.HasValue)
            {
                validTo = validTo.Value.Date;
            }

            if (validTo.HasValue && validTo.Value < validFrom)
            {
                throw new ArgumentException(
                    "Slutdatum får inte vara tidigare än startdatum.");
            }

            // Hämtar hela lönehistoriken för den anställde.
            var salaryHistory =
                await _salaryRepository.GetByEmployeeIdAsync(employeeId);

            var today = DateTime.UtcNow.Date;

            // Hittar löneperioder som överlappar den nya perioden.
            var overlappingSalaries = salaryHistory
                .Where(s =>
                    (validTo == null || s.ValidFrom <= validTo.Value) &&
                    (s.ValidTo == null || s.ValidTo.Value >= validFrom))
                .ToList();

            // En befintlig löneperiod får avslutas automatiskt
            // endast om den redan har börjat gälla.
            // Framtida löneperioder får inte skrivas över automatiskt.
            var previousSalary = overlappingSalaries
                .Where(s => s.ValidFrom <= today &&
                            s.ValidFrom < validFrom)
                .OrderByDescending(s => s.ValidFrom)
                .FirstOrDefault();

            // Alla överlappningar utom den period som ska avslutas
            // innebär en konflikt.
            var conflictingSalaries = overlappingSalaries
                .Where(s => previousSalary == null ||
                            s.Id != previousSalary.Id)
                .ToList();

            if (conflictingSalaries.Any())
            {
                throw new InvalidOperationException(
                    "Den nya löneperioden överlappar med en redan registrerad " +
                    "löneperiod. Kontrollera lönehistoriken innan ändringen registreras.");
            }

            // Avslutar föregående löneperiod dagen före den nya börjar.
            if (previousSalary != null)
            {
                previousSalary.ValidTo = validFrom.AddDays(-1);

                await _salaryRepository.UpdateAsync(previousSalary);
            }

            // Skapar den nya löneposten.
            var newSalary = new Salary
            {
                EmployeeId = employeeId,
                Amount = amount,
                ValidFrom = validFrom,
                ValidTo = validTo,
                CreatedAt = DateTime.UtcNow
            };

            await _salaryRepository.CreateAsync(newSalary);

            // Sparar både den avslutade och den nya löneperioden.
            await _salaryRepository.SaveChangesAsync();

            return newSalary;
        }
    }
}
