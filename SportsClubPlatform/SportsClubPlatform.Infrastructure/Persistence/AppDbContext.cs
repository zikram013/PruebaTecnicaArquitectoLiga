using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsClubPlatform.Domain.Entities;

namespace SportsClubPlatform.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core application database context.
    /// </summary>
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Club> Clubs => Set<Club>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<PlayerContract> PlayerContracts => Set<PlayerContract>();
        public DbSet<Transfer> Transfers => Set<Transfer>();
        public DbSet<TransferAuditEntry> TransferAuditEntries => Set<TransferAuditEntry>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<GeneratedContract> GeneratedContracts => Set<GeneratedContract>();
        public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
