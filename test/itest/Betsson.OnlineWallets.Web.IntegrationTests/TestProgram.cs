namespace Betsson.OnlineWallets.Web.IntegrationTests
{
    internal class TestProgram
    {
        /// <summary>
        /// Arrange
        /// 
        /// No withdrawal or deposit has occured.
        /// 
        /// Act
        /// 
        /// Balance is called.
        /// 
        /// Assert
        /// 
        /// 0 is returned. Status is OK.
        /// </summary>
        [Test]
        public void TestBalance()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Withdraw amount is bigger than 0. Current balance amount is equal to the amount to withdraw.
        /// 
        /// Act
        /// 
        /// Withdraw is called.
        /// 
        /// Assert
        /// 
        /// Current balance is returned with 0. Status is OK.
        /// </summary>
        [Test]
        public void TestWithdraw_AmountIsBiggerThan0()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Withdraw amount is 0.
        /// 
        /// Act
        /// 
        /// Withdraw is called.
        /// 
        /// Assert
        /// 
        /// The action is not executed.
        /// </summary>
        [Test]
        public void TestWithdraw_AmountIs0()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit amount is 0.
        /// 
        /// Act
        /// 
        /// Deposit is called.
        /// 
        /// Assert
        /// 
        /// The action is not executed.
        /// </summary>
        [Test]
        public void TestDeposit_AmountIs0()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit amount is bigger than 0.
        /// 
        /// Act
        /// 
        /// Deposit is called.
        /// 
        /// Assert
        /// 
        /// Current balance is returned with the deposited amount. Status is OK.
        /// </summary>
        [Test]
        public void TestDeposit_AmountIsBiggerThan0()
        {
            Assert.Fail();
        }
    }
}