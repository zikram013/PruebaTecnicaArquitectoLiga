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
    /// EF Core configuration for GeneratedContract entity.
    /// </summary>
    public sealed class GeneratedContractConfiguration : IEntityTypeConfiguration<GeneratedContract>
    {
        public void Configure(EntityTypeBuilder<GeneratedContract> builder)
        {
            builder.ToTable("GeneratedContracts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AnnualSalary)
                .HasPrecision(18, 2);

            builder.Property(x => x.DocumentReference)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.GeneratedAtUtc)
                .IsRequired();

            builder.Property(x => x.IsCancelled)
                .IsRequired();

            builder.HasOne(x => x.Transfer)
                .WithMany()
                .HasForeignKey(x => x.TransferId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
