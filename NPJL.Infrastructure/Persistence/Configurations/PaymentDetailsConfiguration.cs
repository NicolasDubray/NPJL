using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class PaymentDetailsConfiguration
        : IEntityTypeConfiguration<PaymentDetails>
    {
        public void Configure(EntityTypeBuilder<PaymentDetails> builder)
        {
            builder.ToTable("PaymentDetails");

            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(pd => pd.PaymentReference)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(pd => pd.IsActive)
                .IsRequired();

            builder.HasOne(pd => pd.Customer)
                .WithMany(c => c.PaymentDetails)
                .HasForeignKey(pd => pd.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // En kund kan endast ha en betalmetodspost
            builder.HasIndex(pd => pd.CustomerId)
                .IsUnique();
        }
    }
}
