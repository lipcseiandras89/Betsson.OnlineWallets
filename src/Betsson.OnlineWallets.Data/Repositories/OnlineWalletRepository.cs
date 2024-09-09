using Betsson.OnlineWallets.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Betsson.OnlineWallets.IntegrationTests")]
[assembly: InternalsVisibleTo("Betsson.OnlineWallets.Data.IntegrationTests")]
[assembly: InternalsVisibleTo("Betsson.OnlineWallets.Data.UnitTests")]

namespace Betsson.OnlineWallets.Data.Repositories
{
    public class OnlineWalletRepository : IOnlineWalletRepository
    {
        internal static readonly string WALLETCONTEXT_WAS_NULL = "WalletContext was null.";
        internal static readonly string WALLETCONTEXT_TRANSACTIONS_WAS_NULL = "WalletContext.Transactions was null.";
        internal readonly IOnlineWalletContext _onlineWalletContext;

        public OnlineWalletRepository(IOnlineWalletContext onlineWalletContext)
        {
            _onlineWalletContext = onlineWalletContext is null ? throw new ArgumentException(WALLETCONTEXT_WAS_NULL) : 
                onlineWalletContext.Transactions is null ? throw new ArgumentException(WALLETCONTEXT_TRANSACTIONS_WAS_NULL) : onlineWalletContext;
        }

        public async Task<OnlineWalletEntry?> GetLastOnlineWalletEntryAsync()
        {
            return await _onlineWalletContext
                .Transactions
                .OrderByDescending(onlineWalletEntry => onlineWalletEntry.EventTime)
                .FirstOrDefaultAsync();
        }

        public Task? InsertOnlineWalletEntryAsync(OnlineWalletEntry onlineWalletEntry)
        {
            if (onlineWalletEntry == null)
            {
                return null;
            }

            _onlineWalletContext.Transactions.Add(onlineWalletEntry);
            _onlineWalletContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
