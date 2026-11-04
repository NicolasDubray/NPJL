using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class ServiceConfiguration
        : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            // Tabellnamn
            builder.ToTable("Service", table =>
            {
                table.HasCheckConstraint(
                    "CK_Service_Cost",
                    "[Cost] >= 0");
            });

            // Primärnyckel
            builder.HasKey(s => s.Id);

            // Tjänstens namn eller typ
            builder.Property(s => s.Type)
                .HasMaxLength(150)
                .IsRequired();

            // Kostnad
            builder.Property(s => s.Cost)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
