using Betsson.OnlineWallets.Data.Repositories;

namespace Betsson.OnlineWallets.Data.UnitTests.Repositories
{
    [TestFixture]
    internal class TestOnlineWalletRepository
    {
        private readonly IOnlineWalletRepository? _onlineWalletRepository;

        /// <summary>
        /// Arrange
        /// 
        /// onlineWalletEntry is null.
        /// 
        /// Act
        /// 
        /// Call InsertOnlineWalletEntryAsync.
        /// 
        /// Assert
        /// 
        /// InsertOnlineWalletEntryAsync returns null.
        /// </summary>
        [Test]
        public void TestInsertOnlineWalletEntryAsync_OnlineWalletEntryIsNull()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Input data is valid.
        /// 
        /// Act
        /// 
        /// Call InsertOnlineWalletEntryAsync.
        /// 
        /// Assert
        /// 
        /// Transaction is saved after the transaction has been added to the context.
        /// </summary>
        [Test]
        public void TestInsertOnlineWalletEntryAsync_Order()
        {
            Assert.Fail();
        }
    }
}