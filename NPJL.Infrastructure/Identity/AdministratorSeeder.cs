using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace NPJL.Infrastructure.Identity
{
    // Skapar ett testkonto för administratör.
    // Används endast i utvecklingsmiljö.
    public static class AdministratorSeeder
    {
        private const string TestEmail =
            "Testadmind@Testadmind.com";

        private const string TestPassword =
            "Testadmind1!";

        private const string TestRole = "Administrator";

        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager)
        {
            // Kontrollera om administratörskontot redan finns.
            var existingUser = await userManager.FindByEmailAsync(TestEmail);

            if (existingUser != null)
            {
                // Säkerställ att befintligt konto har rätt roll.
                if (!await userManager.IsInRoleAsync(existingUser, TestRole))
                {
                    var roleResult = await userManager.AddToRoleAsync(
                        existingUser,
                        TestRole);

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Could not assign Administrator role: {errors}");
                    }
                }

                return;
            }

            // Skapa ett fristående administratörskonto.
            var user = new ApplicationUser
            {
                UserName = TestEmail,
                Email = TestEmail,
                EmailConfirmed = true,
                EmployeeId = null,
                CustomerId = null
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
                    $"Could not create administrator account: {errors}");
            }

            // Tilldela rollen Administrator.
            var assignRoleResult = await userManager.AddToRoleAsync(
                user,
                TestRole);

            if (!assignRoleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    assignRoleResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Could not assign Administrator role: {errors}");
            }
        }
    }
}
