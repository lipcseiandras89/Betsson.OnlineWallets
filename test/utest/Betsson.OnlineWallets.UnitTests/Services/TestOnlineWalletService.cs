using Betsson.OnlineWallets.Data.Factories;
using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Services;
using Moq;
using Betsson.OnlineWallets.Factories;
using System.ComponentModel.DataAnnotations;

namespace Betsson.OnlineWallets.UnitTests.Services
{
    [TestFixture]
    internal class TestOnlineWalletService
    {
        private IOnlineWalletService _onlineWalletService;

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
        async public Task TestGetBalanceAsync_OnlineWalletEntryIsDefault()
        {
            // Arrange
            Mock<IOnlineWalletRepository> mockRepository = new();

            Task<OnlineWalletEntry?> GetEntry()
            {
                return Task.Run(() => default(OnlineWalletEntry?));
            }

            mockRepository.Setup(mock => mock.GetLastOnlineWalletEntryAsync()).Returns(GetEntry);
            _onlineWalletService = new OnlineWalletService(mockRepository.Object);

            // Act
            Task<Balance> task = _onlineWalletService.GetBalanceAsync();
            Balance balance = await task;

            // Assert
            Assert.That(balance.Amount, Is.Zero);
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and has non-zero and differing 
        /// BalanceBefore and Amount values.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync returns a Balance with an amount, which equals to BalanceBefore + Amount.
        /// </summary>
        [Test]
        async public Task TestGetBalanceAsync_OnlineWalletEntryIsNonDefault()
        {
            sbyte balanceBefore = -3;
            sbyte amount = -4;

            // Arrange
            Mock<IOnlineWalletRepository> mockRepository = new Mock<IOnlineWalletRepository>();

            Task<OnlineWalletEntry?> GetEntry()
            {
                return Task.Run(() => new OnlineWalletEntry
                {
                    BalanceBefore = balanceBefore,
                    Amount = amount,
                });
            }

            mockRepository.Setup(mock => mock.GetLastOnlineWalletEntryAsync()).Returns(GetEntry);
            _onlineWalletService = new OnlineWalletService(mockRepository.Object);

            // Act
            Task<Balance> task = _onlineWalletService.GetBalanceAsync();
            Balance balance = await task;

            // Assert
            Assert.That(balance.Amount, Is.EqualTo(balanceBefore + amount));
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and the sum of it's two values
        /// barely exceed MaxValue of decimal.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync throws exception about the possible overflow.
        /// </summary>
        [Test]
        async public Task TestGetBalanceAsync_BalanceIsBiggerThanMaximum()
        {
            // Arrange
            Mock<IOnlineWalletRepository> mockRepository = new();

            static Task<OnlineWalletEntry> GetEntry()
            {
                return Task.Run(() => new OnlineWalletEntry
                {
                    BalanceBefore = decimal.MaxValue,
                    Amount = 1,
                });
            }

            mockRepository.Setup(mock => mock.GetLastOnlineWalletEntryAsync()).Returns(GetEntry);
            _onlineWalletService = new OnlineWalletService(mockRepository.Object);

            // Act
            var result = await _onlineWalletService.GetBalanceAsync();

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Arrange
        /// 
        /// OnlineWalletRepository.GetLastOnlineWalletEntryAsync returns a value of OnlineWalletEntry which differs from default, and the sum of it's two values is lesser
        /// than MinValue of decimal.
        /// 
        /// Act
        /// 
        /// Call GetBalanceAsync.
        /// 
        /// Assert
        /// 
        /// GetBalanceAsync returns a Balance with an amount, which equals to BalanceBefore + Amount.
        /// </summary>
        [Test]
        async public Task TestGetBalanceAsync_BalanceIsLesserThanMinimum()
        {
            // Arrange
            Mock<IOnlineWalletRepository> mockRepository = new Mock<IOnlineWalletRepository>();

            static Task<OnlineWalletEntry> GetEntry()
            {
                return Task.Run(() => new OnlineWalletEntry
                {
                    BalanceBefore = -1,
                    Amount = decimal.MinValue,
                });
            }

            mockRepository.Setup(mock => mock.GetLastOnlineWalletEntryAsync()).Returns(GetEntry);
            _onlineWalletService = new OnlineWalletService(mockRepository.Object);

            // Act
            var result = await _onlineWalletService.GetBalanceAsync();

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestDepositFundsAsync_OnlineWalletEntryConstructorThrowsException()
        {
            // Arrange
            var mockOnlineWalletEntryFactory = new Mock<OnlineWalletEntryFactory>();
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Throws<Exception>();
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            static Task<Balance> GetBalance() => Task.Run(() => new Balance() { Amount = 0 });

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            _onlineWalletService.EntryFactory = mockOnlineWalletEntryFactory.Object;

            // Act
            var mockDeposit = new Mock<Deposit>();
            mockDeposit.SetupGet(x => x.Amount).Returns(0);
            var result = await _onlineWalletService.DepositFundsAsync(mockDeposit.Object);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Arrange
        /// 
        /// GetBalanceAsync throws exception.
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
        async public Task TestDepositFundsAsync_GetBalanceAsyncThrowsException()
        {
            // Arrange
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Throws<Exception>();
            _onlineWalletService = mockOnlineWalletService.Object;

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(new Mock<Deposit>().Object);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Arrange
        /// 
        /// DepositEntry's BalanceBefore setter throws exception.
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
        async public Task TestDepositFundsAsync_DepositEntryBalanceBeforeThrowsException()
        {
            // Arrange
            var mockOnlineWalletEntryFactory = new Mock<OnlineWalletEntryFactory>();
            var mockOnlineWalletEntry = new Mock<OnlineWalletEntry>();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>()).Throws<Exception>();
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            static Task<Balance> GetBalance() => Task.Run(() => new Balance() { Amount = 0 });

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            _onlineWalletService.EntryFactory = mockOnlineWalletEntryFactory.Object;

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(new Deposit() { Amount = 0 });

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Arrange
        /// 
        /// DepositEntry's EventTime setter throws exception.
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
        async public Task TestDepositFundsAsync_DepositEntryEventTimeThrowsException()
        {
            // Arrange
            var mockOnlineWalletEntryFactory = new Mock<OnlineWalletEntryFactory>();
            var mockOnlineWalletEntry = new Mock<OnlineWalletEntry>();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>()).Throws<Exception>();
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            static Task<Balance> GetBalance() => Task.Run(() => new Balance() { Amount = 0 });

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            _onlineWalletService.EntryFactory = mockOnlineWalletEntryFactory.Object;

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(new Deposit() { Amount = 0 });

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Arrange
        /// 
        /// DepositEntry's amount setter throws exception.
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
        async public Task TestDepositFundsAsync_DepositEntryAmountThrowsException()
        {
            // Arrange
            var mockOnlineWalletEntryFactory = new Mock<OnlineWalletEntryFactory>();
            var mockOnlineWalletEntry = new Mock<OnlineWalletEntry>();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>()).Throws<Exception>();
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            static Task<Balance> GetBalance() => Task.Run(() => new Balance() { Amount = 0 });

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            _onlineWalletService.EntryFactory = mockOnlineWalletEntryFactory.Object;

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(new Deposit() { Amount = 0 });

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestDepositFundsAsync_InsertOnlineWalletEntryAsyncThrowsException()
        {
            // Arrange
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.IsAny<OnlineWalletEntry>())).Throws<Exception>();
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            static Task<Balance> GetBalance() => Task.Run(() => new Mock<Balance>().Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockDeposit = new Mock<Deposit>();
            mockDeposit.SetupGet(x => x.Amount).Returns(0);

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(mockDeposit.Object);

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestDepositFundsAsync_HappyPath()
        {
            // Arrange
            const decimal BALANCE_BEFORE = 1;
            const decimal DEPOSIT_AMOUNT = 2;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.Is<OnlineWalletEntry>(y => y.Amount.Equals(DEPOSIT_AMOUNT)
            && y.BalanceBefore.Equals(BALANCE_BEFORE)))).Verifiable(Times.Once);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<BalanceFactory> mockBalanceFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntry.SetupGet(x => x.Amount).Returns(DEPOSIT_AMOUNT);
            mockOnlineWalletEntry.SetupGet(x => x.BalanceBefore).Returns(BALANCE_BEFORE);
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            Mock<Balance> mockResultBalance = new();
            mockResultBalance.SetupSet(x => x.Amount = It.IsAny<decimal>()).Callback<decimal>(value => mockResultBalance.SetupGet(x => x.Amount).Returns(value));
            mockBalanceFactory.SetupGet(x => x.GetBalance).Returns(mockResultBalance.Object);
            mockOnlineWalletService.SetupGet(x => x.BalanceFactory).Returns(mockBalanceFactory.Object);
            var mockActualBalance = new Mock<Balance>();
            mockActualBalance.SetupGet(x => x.Amount).Returns(BALANCE_BEFORE);
            Task<Balance> GetBalance() => Task.Run(() => mockActualBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockDeposit = new Mock<Deposit>();
            mockDeposit.SetupGet(x => x.Amount).Returns(DEPOSIT_AMOUNT);

            // Act
            var result = await _onlineWalletService.DepositFundsAsync(mockDeposit.Object);

            // Assert
            mockOnlineWalletRepository.Verify();
            Assert.That(result.Amount, Is.EqualTo(DEPOSIT_AMOUNT + BALANCE_BEFORE));
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
        /// Deposit amount is inserted into online wallet without any missing insertions.
        /// </summary>
        [Test]
        [Repeat(25)]
        async public Task TestDepositFundsAsync_TimingInsertion()
        {
            Assert.Fail();
            // Arrange
            const byte LOOP_MAX = 100;
            const decimal BALANCE_BEFORE = 1;
            const decimal DEPOSIT_AMOUNT = 2;
            var balanceActual = BALANCE_BEFORE;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.Is<OnlineWalletEntry>(y => y.Amount.Equals(DEPOSIT_AMOUNT)
            && y.BalanceBefore.Equals(BALANCE_BEFORE)))).Callback(() => balanceActual += DEPOSIT_AMOUNT);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntry.SetupGet(x => x.Amount).Returns(DEPOSIT_AMOUNT);
            mockOnlineWalletEntry.SetupGet(x => x.BalanceBefore).Returns(BALANCE_BEFORE);
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            var mockBalance = new Mock<Balance>();
            mockBalance.SetupGet(x => x.Amount).Returns(BALANCE_BEFORE);
            Task<Balance> GetBalance() => Task.Run(() => mockBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockDeposit = new Mock<Deposit>();
            mockDeposit.SetupGet(x => x.Amount).Returns(DEPOSIT_AMOUNT);

            // Act
            for (byte i = 0; i < LOOP_MAX; i++)
            {
                var result = _onlineWalletService.DepositFundsAsync(mockDeposit.Object);
            }

            _ = await _onlineWalletService.DepositFundsAsync(mockDeposit.Object);

            // Assert
            mockOnlineWalletRepository.Verify(x => x.InsertOnlineWalletEntryAsync(It.Is<OnlineWalletEntry>(y => y.Amount.Equals(DEPOSIT_AMOUNT)
            && y.BalanceBefore.Equals(BALANCE_BEFORE))), Times.Exactly(LOOP_MAX + 1));
        }

        [Test]
        [Repeat(25)]
        public void TestDepositFundsAsync_TimingBalanceResult()
        {
            // Arrange
            const byte LOOP_MAX = 100;
            const decimal BALANCE_STARTING_VALUE = 1;
            const decimal DEPOSIT_STARTING_VALUE = 2;
            decimal depositAmount = DEPOSIT_STARTING_VALUE;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.IsAny<OnlineWalletEntry>()));
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<BalanceFactory> mockBalanceFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntry.SetupGet(x => x.Amount).Returns(depositAmount);
            mockOnlineWalletEntry.SetupGet(x => x.BalanceBefore).Returns(BALANCE_STARTING_VALUE);
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            var mockActualBalance = new Mock<Balance>();
            mockActualBalance.SetupGet(x => x.Amount).Returns(BALANCE_STARTING_VALUE);
            Task<Balance> GetBalance() => Task.Run(() => mockActualBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            List<Task<Balance>> result = [];

            byte i = 0;
            do
            {
                // Arrange
                var mockDeposit = new Mock<Deposit>();
                mockDeposit.SetupGet(x => x.Amount).Returns(depositAmount++);
                Mock<Balance> mockResultBalance = new();    // moved here
                mockResultBalance.SetupSet(x => x.Amount = It.IsAny<decimal>()).Callback<decimal>(value => mockResultBalance.SetupGet(x => x.Amount).Returns(value)); // callback
                mockBalanceFactory = new();
                mockBalanceFactory.SetupGet(x => x.GetBalance).Returns(mockResultBalance.Object); // moved here
                mockOnlineWalletService = new();
                mockOnlineWalletService.SetupGet(x => x.BalanceFactory).Returns(mockBalanceFactory.Object); // moved here

                // Act
                result.Add(_onlineWalletService.DepositFundsAsync(mockDeposit.Object));
            }
            while (i++ < LOOP_MAX);

            // Assert
            depositAmount = DEPOSIT_STARTING_VALUE;
            foreach (Task<Balance> task in result)
            {
                task.Wait();
                Assert.That(task.Result.Amount, Is.EqualTo(BALANCE_STARTING_VALUE + depositAmount++));
            }
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
        async public Task TestWithdrawFundsAsync_GetBalanceAsyncThrowsException()
        {
            // Arrange
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Throws<Exception>();
            _onlineWalletService = mockOnlineWalletService.Object;

            // Act
            var result = await _onlineWalletService.WithdrawFundsAsync(new Mock<Withdrawal>().Object);

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestWithdrawFundsAsync_WithdrawalAmountIsBiggerThanCurrentBalanceAmount()
        {
            // Arrange
            const decimal BALANCE_BEFORE = 0;
            const decimal WITHDRAWAL_AMOUNT = 1;
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<BalanceFactory> mockBalanceFactory = new();
            var mockActualBalance = new Mock<Balance>();
            mockActualBalance.SetupGet(x => x.Amount).Returns(BALANCE_BEFORE);
            Task<Balance> GetBalance() => Task.Run(() => mockActualBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockWithdrawal = new Mock<Withdrawal>();
            mockWithdrawal.SetupGet(x => x.Amount).Returns(WITHDRAWAL_AMOUNT);

            // Act
            var result = await _onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object);

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestWithdrawFundsAsync_OnlineWalletEntryConstructorThrowsException()
        {
            // Arrange
            var mockOnlineWalletEntryFactory = new Mock<OnlineWalletEntryFactory>();
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Throws<Exception>();
            var mockOnlineWalletService = new Mock<OnlineWalletService>(new Mock<IOnlineWalletRepository>().Object)
            {
                CallBase = true
            };

            static Task<Balance> GetBalance() => Task.Run(() => new Balance() { Amount = 0 });

            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            _onlineWalletService.EntryFactory = mockOnlineWalletEntryFactory.Object;

            // Act
            var mockWithdrawal = new Mock<Withdrawal>();
            mockWithdrawal.SetupGet(x => x.Amount).Returns(0);
            var result = await _onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object);

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestWithdrawFundsAsync_InsertOnlineWalletEntryAsyncThrowsException()
        {
            // Arrange
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.IsAny<OnlineWalletEntry>())).Throws<Exception>();
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            static Task<Balance> GetBalance() => Task.Run(() => new Mock<Balance>().Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockWithdrawal = new Mock<Withdrawal>();
            mockWithdrawal.SetupGet(x => x.Amount).Returns(0);

            // Act
            var result = await _onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object);

            // Assert
            Assert.That(result, Is.Null);
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
        async public Task TestWithdrawFundsAsync_HappyPath()
        {
            // Arrange
            const decimal BALANCE_BEFORE = 2;
            const decimal WITHDRAWAL_AMOUNT = 1;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.Is<OnlineWalletEntry>(y => y.Amount.Equals(WITHDRAWAL_AMOUNT)
            && y.BalanceBefore.Equals(BALANCE_BEFORE)))).Verifiable(Times.Once);
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<BalanceFactory> mockBalanceFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntry.SetupGet(x => x.Amount).Returns(WITHDRAWAL_AMOUNT);
            mockOnlineWalletEntry.SetupGet(x => x.BalanceBefore).Returns(BALANCE_BEFORE);
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            Mock<Balance> mockResultBalance = new();
            mockResultBalance.SetupSet(x => x.Amount = It.IsAny<decimal>()).Callback<decimal>(value => mockResultBalance.SetupGet(x => x.Amount).Returns(value));
            mockBalanceFactory.SetupGet(x => x.GetBalance).Returns(mockResultBalance.Object);
            mockOnlineWalletService.SetupGet(x => x.BalanceFactory).Returns(mockBalanceFactory.Object);
            var mockActualBalance = new Mock<Balance>();
            mockActualBalance.SetupGet(x => x.Amount).Returns(BALANCE_BEFORE);
            Task<Balance> GetBalance() => Task.Run(() => mockActualBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            var mockWithdrawal = new Mock<Withdrawal>();
            mockWithdrawal.SetupGet(x => x.Amount).Returns(WITHDRAWAL_AMOUNT);

            // Act
            var result = await _onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object);

            // Assert
            mockOnlineWalletRepository.Verify();
            Assert.That(result.Amount, Is.EqualTo(BALANCE_BEFORE - WITHDRAWAL_AMOUNT));
        }

        /// <summary>
        /// Arrange
        /// 
        /// Arrangement of happy path is applied. Withdraw amount is incrementing with every WithdrawFundsAsync call.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync with the same values a number of times, that could cause timing issues, and therefore false return values.
        /// 
        /// Assert
        /// 
        /// Withdraw amount is inserted into online wallet without any missing insertions.
        /// </summary>
        [Test]
        //[Repeat(25)]
        async public Task TestWithdrawFundsAsync_TimingInsertion()
        {
            // Arrange
            const byte LOOP_MAX = 100;
            const decimal BALANCE_STARTING_VALUE = 10000;
            const decimal WITHDRAW_STARTING_VALUE = 2;
            decimal withdrawAmount = WITHDRAW_STARTING_VALUE;
            decimal balanceAmount = BALANCE_STARTING_VALUE;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            List<decimal> resultAmount = [];
            List<decimal> resultBalance = [];
            Task<Balance?> task;
            byte i = 0;
            do
            {
                // Arrange
                var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
                {
                    CallBase = true
                };
                _onlineWalletService = mockOnlineWalletService.Object;
                mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.IsAny<OnlineWalletEntry>())).Callback<OnlineWalletEntry>(y =>
                {
                    resultBalance.Add(y.BalanceBefore);
                    resultAmount.Add(y.Amount);
                });
                Mock<BalanceFactory> mockBalanceFactory = new();
                var mockBalance = new Mock<Balance>();
                mockBalance.SetupGet(x => x.Amount).Returns(balanceAmount -= withdrawAmount);
                Task<Balance> GetBalance = Task.Run(() => mockBalance.Object);
                mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
                var mockOnlineWalletEntry = new Mock<OnlineWalletEntry>();
                mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>()).Callback<decimal>(x => mockOnlineWalletEntry.SetupGet(y => y.Amount).Returns(x));
                mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>()).Callback<decimal>(x => mockOnlineWalletEntry.SetupGet(y => y.BalanceBefore).Returns(x));
                mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
                Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
                mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
                mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
                var mockWithdrawal = new Mock<Withdrawal>();
                mockWithdrawal.SetupGet(x => x.Amount).Returns(withdrawAmount++);
                
                Mock<Balance> mockResultBalance = new();
                mockResultBalance.SetupSet(x => x.Amount = It.IsAny<decimal>());
                mockBalanceFactory.SetupGet(x => x.GetBalance).Returns(mockResultBalance.Object);
                mockOnlineWalletService = new();
                mockOnlineWalletService.SetupGet(x => x.BalanceFactory).Returns(mockBalanceFactory.Object);

                // Act
                task = _onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object);
            }
            while (i++ < LOOP_MAX);

            // Assert
            withdrawAmount = WITHDRAW_STARTING_VALUE;
            balanceAmount = BALANCE_STARTING_VALUE;
            task.Wait();
            for (int j = 0; j < resultAmount.Count; j++)
            {

                mockOnlineWalletRepository.Verify(x => x.InsertOnlineWalletEntryAsync(It.Is<OnlineWalletEntry>(y =>
                y.BalanceBefore == resultBalance[j] && y.Amount == resultAmount[j])));
                withdrawAmount++;
            }
        }

        /// <summary>
        /// Arrange
        /// 
        /// Arrangement of happy path is applied. Every WithdrawFundsAsync call increments the deposit amount.
        /// 
        /// Act
        /// 
        /// Call WithdrawFundsAsync a number of times, that could cause timing issues, and therefore false return values.
        /// 
        /// Assert
        /// 
        /// WithdrawFundsAsync returns a balance object, which has the correct sum of withdraws.
        /// </summary>
        [Test]
        [Repeat(25)]
        public void TestWithdrawFundsAsync_TimingBalanceResult()
        {
            // Arrange
            const byte LOOP_MAX = 100;
            const decimal BALANCE_STARTING_VALUE = 10000;
            const decimal WITHDRAW_STARTING_VALUE = 2;
            decimal withdrawAmount = WITHDRAW_STARTING_VALUE;
            var mockOnlineWalletRepository = new Mock<IOnlineWalletRepository>();
            mockOnlineWalletRepository.Setup(x => x.InsertOnlineWalletEntryAsync(It.IsAny<OnlineWalletEntry>()));
            var mockOnlineWalletService = new Mock<OnlineWalletService>(mockOnlineWalletRepository.Object)
            {
                CallBase = true
            };
            Mock<OnlineWalletEntryFactory> mockOnlineWalletEntryFactory = new();
            Mock<BalanceFactory> mockBalanceFactory = new();
            Mock<OnlineWalletEntry> mockOnlineWalletEntry = new();
            mockOnlineWalletEntry.SetupSet(x => x.Amount = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.BalanceBefore = It.IsAny<decimal>());
            mockOnlineWalletEntry.SetupSet(x => x.EventTime = It.IsAny<DateTimeOffset>());
            mockOnlineWalletEntry.SetupGet(x => x.Amount).Returns(withdrawAmount);
            mockOnlineWalletEntry.SetupGet(x => x.BalanceBefore).Returns(BALANCE_STARTING_VALUE);
            mockOnlineWalletEntryFactory.SetupGet(x => x.GetOnlineWalletEntry).Returns(mockOnlineWalletEntry.Object);
            mockOnlineWalletService.SetupGet(x => x.EntryFactory).Returns(mockOnlineWalletEntryFactory.Object);
            var mockActualBalance = new Mock<Balance>();
            mockActualBalance.SetupGet(x => x.Amount).Returns(BALANCE_STARTING_VALUE);
            Task<Balance> GetBalance() => Task.Run(() => mockActualBalance.Object);
            mockOnlineWalletService.Setup(x => x.GetBalanceAsync()).Returns(GetBalance);
            _onlineWalletService = mockOnlineWalletService.Object;
            List<Task<Balance>> result = [];

            byte i = 0;
            do
            {
                // Arrange
                var mockWithdrawal = new Mock<Withdrawal>();
                mockWithdrawal.SetupGet(x => x.Amount).Returns(withdrawAmount++);
                Mock<Balance> mockResultBalance = new();
                mockResultBalance.SetupSet(x => x.Amount = It.IsAny<decimal>()).Callback<decimal>(value => mockResultBalance.SetupGet(x => x.Amount).Returns(value));
                mockBalanceFactory = new();
                mockBalanceFactory.SetupGet(x => x.GetBalance).Returns(mockResultBalance.Object);
                mockOnlineWalletService = new();
                mockOnlineWalletService.SetupGet(x => x.BalanceFactory).Returns(mockBalanceFactory.Object);

                // Act
                result.Add(_onlineWalletService.WithdrawFundsAsync(mockWithdrawal.Object));
            }
            while (i++ < LOOP_MAX);

            // Assert
            withdrawAmount = WITHDRAW_STARTING_VALUE;
            foreach (Task<Balance> task in result)
            {
                task.Wait();
                Assert.That(task.Result.Amount, Is.EqualTo(BALANCE_STARTING_VALUE - withdrawAmount++));
            }
        }
    }
}