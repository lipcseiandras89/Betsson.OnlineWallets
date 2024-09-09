using Betsson.OnlineWallets.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Betsson.OnlineWallets.Data
{
    public class OnlineWalletContext : DbContext
    {
        public DbSet<OnlineWalletEntry> Transactions { get; set; }

        public override int SaveChanges() => base.SaveChanges();

        public OnlineWalletContext(DbContextOptions<OnlineWalletContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new OnlineWalletEntryConfiguration());
        }
    }
}
