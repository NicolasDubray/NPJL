using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class ContactInfoConfiguration
        : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            // Tabellnamn
            builder.ToTable("ContactInfo", table =>
            {
                table.HasCheckConstraint(
                    "CK_ContactInfo_ExactlyOneOwner",
                    """
                    ([EmployeeId] IS NOT NULL AND [CustomerId] IS NULL)
                    OR
                    ([EmployeeId] IS NULL AND [CustomerId] IS NOT NULL)
                    """);
            });

            // Primärnyckel
            builder.HasKey(c => c.Id);

            // Kontaktuppgifter
            builder.Property(c => c.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.Telephone)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(254)
                .IsRequired();

            // Relation till Employee
            builder.HasOne(c => c.Employee)
                .WithMany(e => e.ContactInfos)
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation till Customer
            builder.HasOne(c => c.Customer)
                .WithMany(c => c.ContactInfos)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(c => c.EmployeeId);
            builder.HasIndex(c => c.CustomerId);
        }
    }
}
