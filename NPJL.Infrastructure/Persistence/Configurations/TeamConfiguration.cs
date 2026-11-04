using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class TeamConfiguration
        : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // Tabellnamn
            builder.ToTable("Team");

            // Primärnyckel
            builder.HasKey(t => t.Id);
        }
    }
}
