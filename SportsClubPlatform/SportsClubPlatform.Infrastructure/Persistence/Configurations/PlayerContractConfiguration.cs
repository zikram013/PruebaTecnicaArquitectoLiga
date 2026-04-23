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
    /// EF Core configuration for PlayerContract entity.
    /// </summary>
    public sealed class PlayerContractConfiguration : IEntityTypeConfiguration<PlayerContract>
    {
        public void Configure(EntityTypeBuilder<PlayerContract> builder)
        {
            builder.ToTable("PlayerContracts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartDateUtc)
                .IsRequired();

            builder.Property(x => x.EndDateUtc)
                .IsRequired();

            builder.Property(x => x.AnnualSalary)
                .HasPrecision(18, 2);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne(x => x.Player)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Club)
                .WithMany()
                .HasForeignKey(x => x.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
