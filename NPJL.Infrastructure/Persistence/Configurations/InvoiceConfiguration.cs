using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration
        : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // Tabellnamn

            builder.ToTable("Invoices", table =>
            {
                table.HasCheckConstraint(
                    "CK_Invoices_Approval",
                    "([Status] IN ('Approved', 'Sent', 'Paid', 'Expired') " +
                    "AND [ApprovedByUserId] IS NOT NULL AND [ApprovedAt] IS NOT NULL) " +
                    "OR ([Status] IN ('Draft', 'PendingApproval', 'Rejected') " +
                    "AND [ApprovedByUserId] IS NULL AND [ApprovedAt] IS NULL)");
            });


            // Primärnyckel
            builder.HasKey(i => i.Id);

            // Fakturauppgifter
            builder.Property(i => i.OCR)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.SendDate)
                .HasColumnType("date");

            builder.Property(i => i.ExpiryDate)
                .HasColumnType("date");

            // Fakturans status
            builder.Property(i => i.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // Administratörens godkännande
            builder.Property(i => i.ApprovedByUserId)
                .HasMaxLength(450);

            builder.Property(i => i.ApprovedAt)
                .HasColumnType("datetime2");

            // Relation till Customer
            builder.HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation till Identity-användaren som godkänt fakturan
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(i => i.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // OCR-nummer ska vara unikt
            builder.HasIndex(i => i.OCR)
                .IsUnique();

            // Index för kund
            builder.HasIndex(i => i.CustomerId);
        }
    }
}
