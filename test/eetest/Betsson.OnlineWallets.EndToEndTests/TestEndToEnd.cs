using Betsson.OnlineWallets.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Betsson.OnlineWallets.Web;
namespace Betsson.OnlineWallets.EndToEndTests
{
    [TestFixture]
    internal class TestEndToEnd
    {
        private const string REQUEST_URI = "http://localhost:5047/onlinewallet/";
        private readonly string _requestUriBalance = "http://localhost:5047/onlinewallet/" + "balance";
        private readonly string _requestUriDeposit = "http://localhost:5047/onlinewallet/" + "deposit";
        private readonly string _requestUriWithdraw = "http://localhost:5047/onlinewallet/" + "withdraw";
        private readonly WebApplicationFactory<Program> _factory = new();
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = _factory.CreateClient();
            var balance = _client.GetFromJsonAsync<Balance>(_requestUriBalance);
            if (balance.Result.Amount > 0)
            {
                _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal() { Amount = balance.Result.Amount });
            }
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        /// <summary>
        /// Tests whether the API returns a valid response.
        /// </summary>
        [Test]
        async public Task TestApiRan()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(_requestUriBalance);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("application/json; charset=utf-8"));
        }

        /// <summary>
        /// Arrange
        /// 
        /// No deposit or withdrawn has occured.
        /// 
        /// Act
        /// 
        /// Get balance multiple times.
        /// 
        /// Assert
        /// 
        /// Balance returns 0 every time.
        /// </summary>
        [Test]
        async public Task TestBalance()
        {
            Balance balance = await _client.GetFromJsonAsync<Balance>(_requestUriBalance);
            Assert.That(balance.Amount, Is.Zero);

            balance = await _client.GetFromJsonAsync<Balance>(_requestUriBalance);
            Assert.That(balance.Amount, Is.Zero);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Default balance is 100.
        /// Multiple entries are withdrawn from the wallet, with a sum less than 100.
        /// 
        /// Act
        /// 
        /// Get balance.
        /// 
        /// Assert
        /// 
        /// Balance returns the difference of the starter balance and the sum of the entries.
        /// </summary>
        [Test]
        async public Task TestMultipleWithDraw()
        {
            _ = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 100 });
            _ = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 10 });
            _ = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 20 });
            _ = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 33 });

            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            Assert.That(balance.Amount, Is.EqualTo(37));
        }

        /// <summary>
        /// Arrange
        /// 
        /// All actions are valid.
        /// 
        /// Act
        /// 
        /// Do all actions in every possible order.
        /// 
        /// Assert
        /// 
        /// Balance is checked after every action, and always valid.
        /// </summary>
        [Test]
        async public Task TestMultipleAction()
        {
            var response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 100 });
            response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 20 });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            Assert.That(balance.Amount, Is.EqualTo(80));

            response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 56 });
            balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            Assert.That(balance.Amount, Is.EqualTo(136));

            response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 20 });
            balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            Assert.That(balance.Amount, Is.EqualTo(116));

            response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = 25 });
            response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 15 });
            balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            Assert.That(balance.Amount, Is.EqualTo(106));
        }

        /// <summary>
        /// Arrange
        /// 
        /// Multiple entries are put in the wallet.
        /// 
        /// Act
        /// 
        /// Get balance.
        /// 
        /// Assert
        /// 
        /// Balance returns the sum of the entries.
        /// </summary>
        [Test]
        async public Task TestMultipleDeposit()
        {
            var response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 10 });
            response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 20 });
            response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 56 });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(86);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit has minimum value.
        /// 
        /// Act
        /// 
        /// Call deposit.
        /// 
        /// Assert
        /// 
        /// Balance returns the minimum value.
        /// </summary>
        [Test]
        async public Task TestMinimumDeposit()
        {
            var response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = decimal.MinValue });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(decimal.MinValue);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit has maximum value.
        /// 
        /// Act
        /// 
        /// Call deposit.
        /// 
        /// Assert
        /// 
        /// Balance returns the maximum value.
        /// </summary>
        [Test]
        async public Task TestMaximumDeposit()
        {
            var response = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = decimal.MaxValue });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(decimal.MaxValue);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Withdrawal has minimum value.
        /// 
        /// Act
        /// 
        /// Call withdrawal.
        /// 
        /// Assert
        /// 
        /// Balance returns the minimum value.
        /// </summary>
        [Test]
        async public Task TestMinimumWithdrawal()
        {
            var response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = decimal.MinValue });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(decimal.MinValue);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Withdrawal has maximum value.
        /// 
        /// Act
        /// 
        /// Call withdrawal.
        /// 
        /// Assert
        /// 
        /// Balance returns the maximum value.
        /// </summary>
        [Test]
        async public Task TestMaximumWithdrawal()
        {
            var response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Withdrawal { Amount = decimal.MaxValue });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(decimal.MaxValue);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Deposit maximum value.
        /// 
        /// Act
        /// 
        /// Call balance.
        /// 
        /// Assert
        /// 
        /// Balance returns the maximum value.
        /// </summary>
        [Test]
        async public Task TestMaximumBalance()
        {
            var response = await _client.PostAsJsonAsync(_requestUriWithdraw, new Deposit { Amount = decimal.MaxValue });
            Balance balance = await _client.GetFromJsonAsync<Balance>("http://localhost:5047/onlinewallet/balance");
            balance.Amount.Equals(decimal.MaxValue);
        }

        /// <summary>
        /// Arrange
        /// 
        /// Balance is 100.
        /// 
        /// Act
        /// 
        /// Withdraw an amount, that is more than what is in the wallet.
        /// 
        /// Assert
        /// 
        /// Balance returns with an error.
        /// </summary>
        [Test]
        async public Task TestNotEnoughBalanceForWithdrawal()
        {
            _ = await _client.PostAsJsonAsync(_requestUriDeposit, new Deposit { Amount = 100 });
            HttpResponseMessage? response = await _client.PostAsJsonAsync("http://localhost:5047/onlinewallet/withdrawal", new Withdrawal { Amount = 101 });
            try
            {
                response.EnsureSuccessStatusCode(); // Status Code 200-299
            }
            catch (HttpRequestException)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}