using Betsson.OnlineWallets.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Betsson.OnlineWallets.Data
{
    public interface IOnlineWalletContext
    {
        public DbSet<OnlineWalletEntry> Transactions { get; set; }

        int SaveChanges();

        protected void OnModelCreating(ModelBuilder builder) { }
    }
}
