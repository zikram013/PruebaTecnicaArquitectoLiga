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
    /// EF Core configuration for Club entity.
    /// </summary>
    public sealed class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Clubs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(x => x.Budget)
                .WithOne(x => x.Club)
                .HasForeignKey<Budget>(x => x.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
