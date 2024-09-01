namespace Betsson.OnlineWallets.UnitTests.Services
{
    [TestFixture]
    internal class TestOnlineWalletService
    {
        private static readonly object[] _decimalExtremeValues = [
            new decimal[] { -3, -4 },
            new decimal[] { decimal.MaxValue, decimal.MaxValue },
            new decimal[] { decimal.MinValue, decimal.MinValue }
            ];

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns the default of OnlineWalletEntry.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync returns a Balance, which has 0 amount.
        /// </summary>
        [Test]
        public void TestGetBalanceAsync_OnlineWalletEntryIsDefault()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Case 1 : OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and has non-zero and differing 
        /// BalanceBefore and Amount values.
        /// Case 2 : OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and has the maximum values for 
        /// BalanceBefore and Amount.
        /// Case 3 : OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and has the minimum values for 
        /// BalanceBefore and Amount.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync returns a Balance with an amount, which equals to BalanceBefore + Amount.
        /// </summary>
        [TestCaseSource(nameof(_decimalExtremeValues))]
        public void TestGetBalanceAsync_OnlineWalletEntryIsNonDefault(decimal BalanceBefore, decimal Amount)
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Balance constructor throws exception.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync returns null.
        /// </summary>
        [Test]
        public void TestGetBalanceAsync_BalanceConstructorThrowsException()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit is null.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync.
        /// 
        /// Assert
        /// 
        /// DepositFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_DepositIsNull()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// GetBalanceAsync returns null.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync.
        /// 
        /// Assert
        /// 
        /// DepositFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_GetBalanceAsyncReturnsNull()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletEntry constructor throws exception.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync.
        /// 
        /// Assert
        /// 
        /// DepositFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_OnlineWalletEntryConstructorThrowsException()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Balance constructor throws exception.
        /// 
        /// Act
        /// 
        /// Call DepositFundsAsync.
        /// 
        /// Assert
        /// 
        /// DepositFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_BalanceConstructorThrowsException()
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
        /// DepositFundsAsync returns a balance, which is the sum of the given deposit, and the current balance.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_HappyPath()
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
        /// Deposit amount is inserted into online wallet.
        /// DepositFundsAsync returns balance objects, which are the sum of the given deposit, and the current balance. Every amount of the balance objects is unique.
        /// </summary>
        [Test]
        public void TestDepositFundsAsync_Timing()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// WithDrawal is null.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_WithdrawalIsNull()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// GetBalanceAsync throws exception.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_GetBalanceAsyncThrowsException()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Amount of withdrawal is bigger than amount of current balance.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// InsufficientBalanceException is thrown.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_WithdrawalAmountIsBiggerThanCurrentBalanceAmount()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletEntry constructor throws exception.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_OnlineWalletEntryConstructorThrowsException()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// InsertOnlineWalletEntryAsync throws exception.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_InsertOnlineWalletEntryAsyncThrowsException()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Balance constructor throws exception.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns null.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_BalanceConstructorThrowsException()
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
        public void TestWithdrawFundsAsync_HappyPath()
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
        /// Call WithdrawFundsAsync with the same values a number of times, that could cause timing issues, and therefore false return values.
        /// 
        /// Assert
        /// 
        /// Withdraw amount is inserted into online wallet.
        /// WithdrawFundsAsync returns balance objects, which are the sum of the given deposit, and the current balance. Every amount of the balance objects is unique.
        /// </summary>
        [Test]
        public void TestWithdrawFundsAsync_Timing()
        {
            Assert.Fail();
        }
    }
}