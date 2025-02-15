using Microsoft.EntityFrameworkCore;
using RQL2Expression.Core.Models;
using RQL2Expression.Data.Context.Configurations;

namespace RQL2Expression.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHistory> AccountHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("idp");

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHistoryConfiguration());
            DataInitialazer.Init(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
