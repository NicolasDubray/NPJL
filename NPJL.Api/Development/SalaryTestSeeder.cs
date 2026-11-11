using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Api.Development
{
    // Skapar en startlön för testanställd.
    // Används endast vid manuell testning i utvecklingsmiljö.
    public static class SalaryTestSeeder
    {
        public static async Task SeedAsync(
            ISalaryRepository salaryRepository)
        {
            const int employeeId = 1;

            // Kontrollera om testanställd redan har löneuppgifter.
            var existingSalaries =
                await salaryRepository.GetByEmployeeIdAsync(employeeId);

            if (existingSalaries.Any())
            {
                return;
            }

            // Skapar testanställdens första löneperiod.
            var salary = new Salary
            {
                EmployeeId = employeeId,
                Amount = 30000m,
                ValidFrom = new DateTime(2026, 1, 1),
                ValidTo = null,
                CreatedAt = DateTime.UtcNow
            };

            await salaryRepository.CreateAsync(salary);

            // Sparar startlönen i databasen.
            await salaryRepository.SaveChangesAsync();
        }
    }
}