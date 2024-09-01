namespace Betsson.OnlineWallets.Data.IntegrationTests.Repositories
{
    internal class TestOnlineWalletRepository
    {
        /// <summary>
        /// Arrange
        /// 
        /// There are no transactions in the context.
        /// 
        /// Act
        /// 
        /// Call GetLastOnlineWalletEntryAsync.
        /// 
        /// Assert
        /// 
        /// GetLastOnlineWalletEntryAsync returns null.
        /// </summary>
        [Test]
        public void TestGetLastOnlineWalletEntryAsync_NoEntry()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// One entry is in the context.
        /// 
        /// Act
        /// 
        /// Call GetLastOnlineWalletEntryAsync.
        /// 
        /// Assert
        /// 
        /// GetLastOnlineWalletEntryAsync returns the only entry.
        /// </summary>
        [Test]
        public void TestGetLastOnlineWalletEntryAsync_OneEntry()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// Multiple entries are in the context.
        /// 
        /// Act
        /// 
        /// Call GetLastOnlineWalletEntryAsync.
        /// 
        /// Assert
        /// 
        /// GetLastOnlineWalletEntryAsync returns the last entry based on EventTime.
        /// </summary>
        [Test]
        public void TestGetLastOnlineWalletEntryAsync_MultipleEntries()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Arrange
        /// 
        /// All input values are valid. Entries are created in reversed order compared to the insertion into repository.
        /// 
        /// Act
        /// 
        /// Call InsertOnlineWalletEntryAsync multiple (at least 3) times.
        /// 
        /// Assert
        /// 
        /// Task is completed. GetLastOnlineWalletEntryAsync returns the last inserted entry.
        /// </summary>
        [Test]
        public void TestInsertOnlineWalletEntryAsync_MultipleEntry()
        {
            Assert.Fail();
        }
    }
}