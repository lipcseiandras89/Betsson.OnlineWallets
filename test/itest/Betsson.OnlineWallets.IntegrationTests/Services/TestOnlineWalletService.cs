namespace Betsson.OnlineWallets.IntegrationTests.Services
{
    internal class TestOnlineWalletService
    {
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
    }
}