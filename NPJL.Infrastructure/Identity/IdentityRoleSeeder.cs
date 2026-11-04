using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace NPJL.Infrastructure.Identity
{
    public static class IdentityRoleSeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            // Roller som ska finnas i systemet
            string[] roles =
            {
                "Customer",
                "Employee",
                "Administrator"
            };

            foreach (var role in roles)
            {
                // Kontrollera om rollen redan finns
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Could not create role '{role}': {errors}");
                    }
                }
            }
        }
    }
}
