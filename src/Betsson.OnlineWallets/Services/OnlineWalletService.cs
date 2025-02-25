using Betsson.OnlineWallets.Data.Factories;
using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Exceptions;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Factories;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Betsson.OnlineWallets.UnitTests")]
[assembly: InternalsVisibleTo("Betsson.OnlineWallets.IntegrationTests")]
// needed for mocking
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Betsson.OnlineWallets.Services
{
    internal class OnlineWalletService(IOnlineWalletRepository onlineWalletRepository) : IOnlineWalletService
    {
        private static readonly SemaphoreSlim semaphoreDeposit = new(1);
        private static readonly SemaphoreSlim semaphoreWithdraw = new(1);
        private readonly IOnlineWalletRepository _onlineWalletRepository = onlineWalletRepository;

        public virtual OnlineWalletEntryFactory EntryFactory { get; set; } = new OnlineWalletEntryFactory();

        public virtual BalanceFactory BalanceFactory { get; set; } = new BalanceFactory();

        public async virtual Task<Balance> GetBalanceAsync()
        {
            OnlineWalletEntry? onlineWalletEntry = await _onlineWalletRepository.GetLastOnlineWalletEntryAsync();

            decimal amount;
            try
            {
                // Default BalanceBefore to 0 if there are no transactions
                amount = onlineWalletEntry == default(OnlineWalletEntry) ? 0 : (onlineWalletEntry.BalanceBefore + onlineWalletEntry.Amount);
            }
            catch (OverflowException)
            {
                // TODO implement error message logic
                return null;
            }

            Balance currentBalance = new()
            {
                Amount = amount
            };

            return currentBalance;
        }

        public async virtual Task<Balance?> DepositFundsAsync(Deposit deposit)
        {
            semaphoreDeposit.Wait();
            decimal depositAmount = deposit.Amount;
            OnlineWalletEntry? depositEntry;
            decimal currentBalanceAmount;
            try
            {
                Balance currentBalance = await GetBalanceAsync();
                currentBalanceAmount = currentBalance.Amount;
                depositEntry = EntryFactory.GetOnlineWalletEntry;
                depositEntry.Amount = depositAmount;
                depositEntry.BalanceBefore = currentBalanceAmount;
                depositEntry.EventTime = DateTimeOffset.UtcNow;
                await _onlineWalletRepository.InsertOnlineWalletEntryAsync(depositEntry);
            }
            catch
            {
                // TODO implement error message logic
                semaphoreDeposit.Release();
                return null;
            }

            Balance newBalance = BalanceFactory.GetBalance;
            newBalance.Amount = currentBalanceAmount + depositAmount;
            semaphoreDeposit.Release();

            return newBalance;
        }

        public async virtual Task<Balance?> WithdrawFundsAsync(Withdrawal withdrawal)
        {
            semaphoreWithdraw.Wait();
            decimal withdrawalAmount = withdrawal.Amount;
            decimal currentBalanceAmount;
            OnlineWalletEntry withdrawalEntry;
            try
            {
                Balance currentBalance = await GetBalanceAsync();
                currentBalanceAmount = currentBalance.Amount;
                if (withdrawalAmount > currentBalance.Amount)
                {
                    throw new InsufficientBalanceException();
                }

                withdrawalAmount *= -1;
                withdrawalEntry = EntryFactory.GetOnlineWalletEntry;
                withdrawalEntry.Amount = withdrawalAmount;
                withdrawalEntry.BalanceBefore = currentBalanceAmount;
                withdrawalEntry.EventTime = DateTimeOffset.UtcNow;
                await _onlineWalletRepository.InsertOnlineWalletEntryAsync(withdrawalEntry);
            }
            catch
            {
                // TODO implement error message logic
                semaphoreWithdraw.Release();
                return null;
            }


            Balance newBalance = BalanceFactory.GetBalance;
            newBalance.Amount = currentBalanceAmount + withdrawalAmount;
            semaphoreWithdraw.Release();

            return newBalance;
        }
    }
}
