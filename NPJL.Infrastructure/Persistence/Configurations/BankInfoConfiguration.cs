using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class BankInfoConfiguration
        : IEntityTypeConfiguration<BankInfo>
    {
        public void Configure(EntityTypeBuilder<BankInfo> builder)
        {
            // Tabellnamn
            builder.ToTable("BankInfo");

            // Primärnyckel
            builder.HasKey(b => b.Id);

            // Bankuppgifter
            builder.Property(b => b.AccountHolderName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(b => b.AccountHolderType)
            .HasConversion<string>()
              .HasMaxLength(20)
              .IsRequired();

            builder.Property(b => b.BankName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.BankAccountNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(b => b.ClearingNumber)
                .HasMaxLength(20)
                .IsRequired();
            
            // Relation till Employee
            builder.HasOne(b => b.Employee)
                .WithMany(e => e.BankInfos)
                .HasForeignKey(b => b.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för anställd
            builder.HasIndex(b => b.EmployeeId);
        }
    }
}
