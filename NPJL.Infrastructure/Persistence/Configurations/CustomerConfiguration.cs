using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration
        : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", table =>
            {
                table.HasCheckConstraint(
                    "CK_Customers_CompanyName",
                    """
        [IsCompany] = 0 OR
        (
            [CompanyName] IS NOT NULL
            AND LEN(LTRIM(RTRIM([CompanyName]))) > 0
        )
        """);
            });
           
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.CompanyName)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(c => c.IsActive)
                .IsRequired();

            builder.Property(c => c.IsCompany)
                .IsRequired();
        }
    }
}
