using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SpaceGoDbContext : DbContext
    {
        public SpaceGoDbContext(DbContextOptions<SpaceGoDbContext> options) : base(options) { }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Slot> Slots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasData(new Wallet { Id = Guid.NewGuid(), PlayerId = "9003", Balance = 200 });
            modelBuilder.Entity<Slot>().HasData(new Slot { Id = Guid.NewGuid(), PlayerId = "9003", LastStop = [0, 1, 2, 3, 4] });
        }
    }
}