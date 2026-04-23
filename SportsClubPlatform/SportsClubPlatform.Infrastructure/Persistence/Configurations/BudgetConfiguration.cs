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
    /// EF Core configuration for Budget entity.
    /// </summary>
    public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AvailableAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.LastUpdatedUtc)
                .IsRequired();

            builder.HasIndex(x => x.ClubId)
                .IsUnique();
        }
    }
}
