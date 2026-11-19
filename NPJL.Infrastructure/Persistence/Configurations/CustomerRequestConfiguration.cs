using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations;

public class CustomerRequestConfiguration : IEntityTypeConfiguration<CustomerRequest>
{
    public void Configure(EntityTypeBuilder<CustomerRequest> builder)
    {
        builder.ToTable("Customer_Request", table =>
        {
            table.HasCheckConstraint("CK_Customer_Request_Type", "[Type] IN ('Cancel', 'Update', 'Question')");
            table.HasCheckConstraint(
                "CK_Customer_Request_ExactlyOneTarget",
                "([BookingId] IS NOT NULL AND [InvoiceId] IS NULL) OR ([BookingId] IS NULL AND [InvoiceId] IS NOT NULL)");
        });

        builder.HasKey(cr => cr.Id);
        builder.Property(cr => cr.CreatedAt).IsRequired();
        builder.Property(cr => cr.Description).IsRequired().HasMaxLength(1000);
        builder.Property(cr => cr.IsReviewed).IsRequired();
        builder.Property(cr => cr.Type).HasConversion<string>()
            .HasMaxLength(20).IsRequired();
        builder.HasIndex(cr => cr.CustomerId);

        builder.HasOne(r => r.Customer).WithMany()
            .HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Booking).WithMany()
            .HasForeignKey(r => r.BookingId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Invoice).WithMany()
            .HasForeignKey(r => r.InvoiceId).OnDelete(DeleteBehavior.Restrict);
    }
}