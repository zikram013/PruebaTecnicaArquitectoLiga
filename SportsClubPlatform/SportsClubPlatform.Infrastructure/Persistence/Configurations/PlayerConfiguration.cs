using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Domain.Entities;

namespace SportsClubPlatform.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for Player entity.
    /// </summary>
    public sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.MarketValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.DateOfBirthUtc)
                .IsRequired();

            builder.HasOne(x => x.CurrentClub)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.CurrentClubId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
