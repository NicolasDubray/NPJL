using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Identity
{
    // Skapar en testanställd och ett tillhörande Identity-konto.
    // Används endast i utvecklingsmiljö.
    public static class TestEmployeeSeeder
    {
        private const string TestEmail =
            "Testanstalld@Testanstalld.com";

        private const string TestPassword =
            "Testanställd1!";

        private const string TestRole = "Employee";

        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            // Kontrollera om testkontot redan finns.
            var existingUser = await userManager.FindByEmailAsync(TestEmail);

            if (existingUser != null)
            {
                return;
            }

            // Startar en transaktion så att seedningen kan
            // återställas om något steg misslyckas.
            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                // Unikt testpersonnummer för att undvika dubbletter.
                const string testSsn = "TEST-EMP-0001";

                // Kontrollera om testanställden redan finns.
                var employee = await context.Employees
                    .Include(e => e.ContactInfos)
                    .FirstOrDefaultAsync(e => e.SSN == testSsn);

                // Skapa testanställd om den inte finns.
                if (employee == null)
                {
                    employee = new Employee
                    {
                        FirstName = "Test",
                        LastName = "Anställd1",
                        SSN = testSsn,
                        IsActive = true,
                        VacationDays = 25
                    };

                    await context.Employees.AddAsync(employee);
                    await context.SaveChangesAsync();
                }

                // Skapa kontaktuppgifter om de saknas.
                if (!employee.ContactInfos.Any())
                {
                    var contactInfo = new ContactInfo
                    {
                        EmployeeId = employee.Id,
                        Address = "Testgatan 1",
                        Telephone = "0700000000",
                        Email = TestEmail
                    };

                    await context.ContactInfos.AddAsync(contactInfo);
                }

                await context.SaveChangesAsync();

                // Kontrollera att testrollen finns.
                if (!await userManager.IsInRoleAsync(
                    new ApplicationUser(),
                    TestRole))
                {
                    // Rollen kontrolleras separat nedan.
                }

                // Skapa Identity-kontot och koppla det till anställd.
                var user = new ApplicationUser
                {
                    UserName = TestEmail,
                    Email = TestEmail,
                    EmailConfirmed = true,
                    EmployeeId = employee.Id
                };

                var createResult = await userManager.CreateAsync(
                    user,
                    TestPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Could not create test employee account: {errors}");
                }

                // Tilldela rollen Employee.
                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    TestRole);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Could not assign Employee role: {errors}");
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
