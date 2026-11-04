using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class TeamEmployeeConfiguration
        : IEntityTypeConfiguration<TeamEmployee>
    {
        public void Configure(EntityTypeBuilder<TeamEmployee> builder)
        {
            builder.ToTable("Team_Employees");

            // Sammansatt primärnyckel
            builder.HasKey(te => new { te.TeamId, te.EmployeeId });

            // Relation till Team
            builder.HasOne(te => te.Team)
                .WithMany(t => t.TeamEmployees)
                .HasForeignKey(te => te.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation till Employee
            builder.HasOne(te => te.Employee)
                .WithMany(e => e.TeamEmployees)
                .HasForeignKey(te => te.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

