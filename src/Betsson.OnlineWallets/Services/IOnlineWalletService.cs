using Betsson.OnlineWallets.Data.Factories;
using Betsson.OnlineWallets.Models;

namespace Betsson.OnlineWallets.Services
{
    public interface IOnlineWalletService
    {
        OnlineWalletEntryFactory EntryFactory { get; set; }

        Task<Balance> GetBalanceAsync();

        Task<Balance?> DepositFundsAsync(Deposit deposit);

        Task<Balance?> WithdrawFundsAsync(Withdrawal withdrawal);
    }
}
