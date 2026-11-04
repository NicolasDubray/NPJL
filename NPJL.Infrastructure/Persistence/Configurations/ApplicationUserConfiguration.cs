using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Infrastructure.Identity;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration
        : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // En användare måste vara kopplad till antingen
            // en kund eller en anställd.
            builder.ToTable("AspNetUsers", table =>
            {
                table.HasCheckConstraint(
                    "CK_AspNetUsers_ExactlyOneOwner",
                    """
                    (
                        [CustomerId] IS NOT NULL
                        AND [EmployeeId] IS NULL
                    )
                    OR
                    (
                        [CustomerId] IS NULL
                        AND [EmployeeId] IS NOT NULL
                    )
                    """);
            });

            // Ett kundkonto per kund
            builder.HasOne(u => u.Customer)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ett anställdkonto per anställd
            builder.HasOne(u => u.Employee)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
