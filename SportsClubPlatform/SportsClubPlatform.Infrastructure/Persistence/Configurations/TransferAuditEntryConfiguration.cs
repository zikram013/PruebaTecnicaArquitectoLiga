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
    /// EF Core configuration for TransferAuditEntry entity.
    /// </summary>
    public sealed class TransferAuditEntryConfiguration : IEntityTypeConfiguration<TransferAuditEntry>
    {
        public void Configure(EntityTypeBuilder<TransferAuditEntry> builder)
        {
            builder.ToTable("TransferAuditEntries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Step)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Message)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.HasOne(x => x.Transfer)
                .WithMany()
                .HasForeignKey(x => x.TransferId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
