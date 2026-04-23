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
    /// EF Core configuration for Transfer entity.
    /// </summary>
    public sealed class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable("Transfers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OfferAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.SalaryProposed)
                .HasPrecision(18, 2);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.LastUpdatedUtc)
                .IsRequired();

            builder.HasOne<Player>()
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Club>()
                .WithMany()
                .HasForeignKey(x => x.SourceClubId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Club>()
                .WithMany()
                .HasForeignKey(x => x.DestinationClubId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
