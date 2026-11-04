using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Persistence.Configurations
{
    public class BookingTeamConfiguration
        : IEntityTypeConfiguration<BookingTeam>
    {
        public void Configure(EntityTypeBuilder<BookingTeam> builder)
        {
            builder.ToTable("Bookings_Team");

            // Sammansatt primärnyckel
            builder.HasKey(bt => new { bt.BookingId, bt.TeamId });

            // Relation till Booking
            builder.HasOne(bt => bt.Booking)
                .WithMany(b => b.BookingTeams)
                .HasForeignKey(bt => bt.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation till Team
            builder.HasOne(bt => bt.Team)
                .WithMany(t => t.BookingTeams)
                .HasForeignKey(bt => bt.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
