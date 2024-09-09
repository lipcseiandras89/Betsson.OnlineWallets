using Betsson.OnlineWallets.Data.Factories;
using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Services;
using Moq;
using Betsson.OnlineWallets.Factories;
using Betsson.OnlineWallets.Data;
using Microsoft.EntityFrameworkCore;

namespace Betsson.OnlineWallets.IntegrationTests.Services
{
    internal class TestOnlineWalletService
    {
        private IOnlineWalletService _onlineWalletService;

        /// <summary>
        /// Arrange
        /// 
        /// No deposit or withdraw has happened.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// Balance is returned.
        /// </summary>
        [Test]
        public void TestGetBalanceAsync()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// All input values are valid, and none of the calls, properties, or constructors throw exception.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync.
        /// 
        /// Assert
        /// 
        /// Deposit amount is inserted into online wallet.
        /// DepositFundsAsync returns a balance, which is the difference of the current balance, and the given withdraw.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// All input values are valid, and none of the calls, properties, or constructors throw exception.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// Withdraw amount is inserted into online wallet.
        /// WithdrawFundsAsync returns a balance, which is the difference of the current balance, and the given withdraw.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Arrangement of happy path is applied.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync a number of times, that could cause timing issues, and therefore false return values.
        /// 
        /// Assert
        /// 
        /// DepositFundsAsync returns balance objects, which are the sum of the given deposit, and the current balance.
        /// </summary>
        [Test]
        [Repeat(25)]
        async public Task TestDepositFundsAsync_TimingBalanceResult()
        {
            //// Arrange
            //const byte LOOP_MAX = 100;
            //const byte BALANCE_BEFORE = 1;
            //byte depositAmount = 2;
            //var balanceActual = BALANCE_BEFORE;
            //_onlineWalletService = new OnlineWalletService(new OnlineWalletRepository(new OnlineWalletContext(new DbContextOptions<OnlineWalletContext>())));

            //// Act
            //for (byte i = 0; i < LOOP_MAX; i++)
            //{
            //    _ = _onlineWalletService.DepositFundsAsync(new Deposit() { Amount = depositAmount++ });
            //}

            //_ = await _onlineWalletService.DepositFundsAsync(new Deposit() { Amount = depositAmount });

            //// Assert
            //Assert.That(balanceActual, Is.EqualTo(Enumerable.Range(1, LOOP_MAX + 1).Aggregate(2, (p, item) => p + item) + BALANCE_BEFORE));
        }
    }
}