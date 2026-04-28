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
    /// EF Core configuration for NotificationLog entity.
    /// </summary>
    public sealed class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
    {
        public void Configure(EntityTypeBuilder<NotificationLog> builder)
        {
            builder.ToTable("NotificationLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RecipientType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Message)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.SentAtUtc)
                .IsRequired();

            builder.HasOne(x => x.Transfer)
                .WithMany()
                .HasForeignKey(x => x.TransferId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
