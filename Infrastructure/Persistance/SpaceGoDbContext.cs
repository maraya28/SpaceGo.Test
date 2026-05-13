using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class SpaceGoDbContext : DbContext
    {
        public SpaceGoDbContext(DbContextOptions<SpaceGoDbContext> options) : base(options) { }

        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasData(new Wallet { Id = Guid.NewGuid(), PlayerId = "9003", Balance = 200 });
        }
    }
}