using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Linq.Expressions;

namespace Betsson.OnlineWallets.Data.UnitTests.Repositories
{
    [TestFixture]
    internal class TestOnlineWalletRepository
    {
        private IOnlineWalletRepository? _onlineWalletRepository;

        /// <summary>
        /// Arrange
        /// 
        /// onlineWalletContext is null.
        /// 
        /// Act
        /// 
        /// Construct OnlineWalletRepository.
        /// 
        /// Assert
        /// 
        /// Constructor throws ArgumentException.
        /// </summary>
        [Test]
        public void TestConstructor_OnlineWalletContextIsNull()
        {
            // Arrange
            OnlineWalletContext context = null;
            Exception ex;

            // Act
            TestDelegate construct = () => new OnlineWalletRepository(context);

            // Act and assert
            Assert.Throws<ArgumentException>(construct, OnlineWalletRepository.WALLETCONTEXT_WAS_NULL);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Transactions in onlineWalletContext is null.
        /// 
        /// Act
        /// 
        /// Construct OnlineWalletRepository.
        /// 
        /// Assert
        /// 
        /// Constructor throws ArgumentException.
        /// </summary>
        [Test]
        public void TestConstructor_TransactionsIsNull()
        {
            // Arrange
            Mock<OnlineWalletContext> mockContext = new();
            mockContext.SetupGet(x => x.Transactions).Returns(null as DbSet<OnlineWalletEntry>);

            // Act
            TestDelegate construct = () => new OnlineWalletRepository(mockContext.Object);

            // Assert
            Assert.Throws<ArgumentException>(construct, OnlineWalletRepository.WALLETCONTEXT_TRANSACTIONS_WAS_NULL);
        }

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
            // Arrange
            Mock<OnlineWalletContext> mockContext = new();
            mockContext.SetupGet(x => x.Transactions).Returns(new Mock<DbSet<OnlineWalletEntry>>().Object);
            _onlineWalletRepository = new OnlineWalletRepository(mockContext.Object);
            OnlineWalletEntry onlineWalletEntry = null;

            // Act
            var result = _onlineWalletRepository.InsertOnlineWalletEntryAsync(onlineWalletEntry);

            // Assert
            Assert.IsNull(result);
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
            // Arrange
            Mock<OnlineWalletContext> mockContext = new();
            Mock<DbSet<OnlineWalletEntry>> mockTransaction = new();
            mockContext.SetupGet(x => x.Transactions).Returns(mockTransaction.Object);
            _onlineWalletRepository = new OnlineWalletRepository(mockContext.Object);
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            Expression<Func<DbSet<OnlineWalletEntry>, EntityEntry<OnlineWalletEntry>>> mockTransactionAction = x => x.Add(It.Is<OnlineWalletEntry>(y => y.Equals(mockOnlineWalletEntry.Object)));
            Expression<Func<OnlineWalletContext, int>> mockContextAction = x => x.SaveChanges();
            bool transactionAdded = false;
            mockContext.Setup(mockContextAction).Returns(0).Callback(() =>
            {
                if (!transactionAdded) 
                {
                    Assert.Fail(); 
                }
            });
            mockTransaction.Setup(mockTransactionAction).Returns(It.IsAny<EntityEntry<OnlineWalletEntry>>).Callback(() => transactionAdded = true);

            // Act
            _onlineWalletRepository.InsertOnlineWalletEntryAsync(mockOnlineWalletEntry.Object);

            // Assert
            mockTransaction.Verify(mockTransactionAction, Times.Once());
            mockContext.Verify(mockContextAction, Times.Once());
        }
    }
}